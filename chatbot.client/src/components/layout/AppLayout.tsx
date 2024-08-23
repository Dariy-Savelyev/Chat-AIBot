import { Button, Input, Layout, Menu, Typography } from 'antd';
import Sider from 'antd/es/layout/Sider';
import { ChangeEvent, createContext, useCallback, useEffect, useState } from 'react';
import type { MenuProps } from 'antd';
import { ChatNameModel } from '../../models/ChatNameModel';
import { get, post } from '../../services/ApiClient';
import { GetAllChatModel } from '../../models/GetAllChatModel';
import { useDispatch, useSelector } from 'react-redux';
import { setChats } from '../../store/slices/ChatSlice';
import { ChatStateModel } from '../../store/models/ChatStateModel';
import { Link, useLocation } from 'react-router-dom';
import '../../assets/styles/appLayout.css';
import { AccesTokenService } from '../../services/AccessTokenService';
import { AppLayoutProps } from '../../models/PropsModels/AppLayoutProps';
import { getChatsWithJoinedStatus } from '../../store/selectors/Selectors';

export const UserContext = createContext('');

const { Header, Content, Footer } = Layout;
const { Text } = Typography;

const AppLayout = ({ children }: AppLayoutProps) => {
  const [isLoggedIn, setIsLoggedIn] = useState(AccesTokenService.isLoggedIn());
  const [showInput, setShowInput] = useState(false);
  const [userId, setUserId] = useState('');

  const [chatName, setFormData] = useState<ChatNameModel>({
    name: ''
  });

  const chats = useSelector((state: ChatStateModel) => getChatsWithJoinedStatus(state, userId));
  const isHomePage = !['/registration', '/login'].includes(useLocation().pathname);

  const dispatch = useDispatch();

  const onLogout = useCallback(async () => {
    await post<void>('/api/tokens/revoke', {});

    AccesTokenService.revokeAccessToken();

    setIsLoggedIn(false);
  }, []);

  const handleChange = useCallback((e: ChangeEvent<HTMLInputElement>) => {
    setFormData({
      ...chatName,
      name: e.target.value
    });
  }, []);

  const showChatInput = useCallback(() => {
    setShowInput(true);
  }, []);

  const submitChatName = useCallback(async () => {
    try {
      const response = await post<string>('/api/chat/create', chatName);

      const chatId = {
        chatId: +response
      }

      await post<string>('/api/chat/join', chatId);

      dispatch(setChats([{ id: +response, name: chatName.name, joined: true, userIds: [userId] }, ...Object.values(chats).flat()]));
    }
    finally {
      setFormData({ name: '' });
      setShowInput(false);
    }
  }, [chats, chatName, dispatch]);

  const onReload = useCallback(async () => {
    const userId = await get<string>('/api/account/userInfo');
    setUserId(userId);

    const fetchedChats = await get<GetAllChatModel[]>('/api/chat/getAllChats');
    dispatch(setChats(fetchedChats));
  }, [dispatch]);

  const joinChat = useCallback(async (chatId: number) => {
    const joinChatId = {
      chatId: chatId
    }

    await post<string>('/api/chat/join', joinChatId);
  }, [dispatch, chats]);

  useEffect(() => {
    if (isHomePage) {
      setIsLoggedIn(AccesTokenService.isLoggedIn());
      if (isLoggedIn) {
        onReload();
      }
    }
  }, [isHomePage, isLoggedIn, onReload]);

  const userChats: MenuProps['items'] = Object.values(chats).flat().map(
    (chat) => {
      return {
        key: chat.id,
        label:
          <>
            <Link to={`/chat/${chat.id}`}>{chat.name}</Link>
            {!chat.joined &&
              <Button
                className='button-join'
                type='primary'
                onClick={() => joinChat(chat.id)}
                href={`/chat/${chat.id}`}
              >
                Join
              </Button>
            }
          </>
      };
    },
  );

  const navigationItems = [
    {
      label: <Link to='/'>Home</Link>,
      key: 'home',
    },
    {
      label: <Link to='/registration'>Registration</Link>,
      key: 'registration',
    },
    {
      label: isLoggedIn ? (
        <Link to='/login' onClick={onLogout}>Logout</Link>
      ) : (
        <Link to='/login'>Login</Link>
      ),
      key: 'login',
    },
  ];

  return (
    <UserContext.Provider value={userId}>
      <Layout>
        <Header>
          <Menu
            className='menu'
            theme='dark'
            mode='horizontal'
            items={navigationItems}
            selectable={false}
          />
        </Header>

        <Content>
          <Layout>
            {(isHomePage && isLoggedIn) && (
              <>
                <Sider className='sider-width'>
                  {showInput ? (
                    <>
                      <Input
                        className='input'
                        type='text'
                        value={chatName.name}
                        onChange={handleChange}
                        placeholder='Enter chat name'
                      />
                      <Button
                        className='button-input'
                        type='primary'
                        htmlType='submit'
                        onClick={submitChatName}
                        disabled={chatName.name.trim() === ''}
                      >
                        Create
                      </Button>
                    </>
                  ) : (
                    <Button className='button' type='primary' onClick={showChatInput}>Create Chat</Button>
                  )}
                  <Menu
                    className='hover-scrollbar'
                    mode='inline'
                    items={userChats}
                    selectedKeys={[location.pathname.split('/')[2]]}
                  />
                </Sider>
              </>
            )}
            <Content className='fit-content'>{children}</Content>
          </Layout>
        </Content>

        <Footer className='footer'>Chat Bot© {new Date().getFullYear()} <Text code>v.{process.env.APP_VERSION}</Text></Footer>
      </Layout>
    </UserContext.Provider>
  );
};

export default AppLayout;