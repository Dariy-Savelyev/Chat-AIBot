import { Button, Input, Layout, Menu } from 'antd';
import Sider from 'antd/es/layout/Sider';
import Link from 'antd/es/typography/Link';
import { ChangeEvent, useCallback, useEffect, useState } from 'react';
import type { MenuProps } from 'antd';
import { ChatNameModel } from '../../models/ChatNameModel';
import { get, post } from '../../services/ApiClient';
import { GetAllChatModel } from '../../models/GetAllChatModel';
import { useDispatch, useSelector } from 'react-redux';
import { setChats } from '../../store/slices/ChatSlice';
import { ChatStateModel } from '../../store/models/ChatStateModel';
import { useLocation } from 'react-router-dom';
import '../../assets/styles/appLayout.css';
import { AccesTokenService } from '../../services/AccessTokenService';
import { AppLayoutProps } from '../../models/AppLayoutPropsModel';

const { Header, Content, Footer } = Layout;

const AppLayout = ({ children }: AppLayoutProps) => {
  const [showInput, setShowInput] = useState(false);

  const [chatName, setFormData] = useState<ChatNameModel>({
    name: ''
  });

  const chats = useSelector((state: ChatStateModel) => state.chats);
  const isHomePage = !['/registration', '/login'].includes(useLocation().pathname);
  const isLoggedIn = AccesTokenService.isLoggedIn();

  const dispatch = useDispatch();

  const onLogout = useCallback(async () => {
    await post<void>('/api/tokens/revoke', {});

    AccesTokenService.revokeAccessToken();
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

      dispatch(setChats([{ id: +response, name: chatName.name, joined: true }, ...Object.values(chats).flat()]));
    }
    finally {
      setFormData({ name: '' });
      setShowInput(false);
    }
  }, [chats, chatName, dispatch]);

  const onReload = useCallback(async () => {
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
    if (isHomePage && isLoggedIn) {
      onReload();
    }
  }, [isHomePage]);

  const userChats: MenuProps['items'] = Object.values(chats).flat().map(
    (chat) => {
      return {
        key: chat.id,
        label:
          <>
            <Link href={`/chat/${chat.id}`}>{chat.name}</Link>
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
      label: <Link href='/'>Home</Link>,
      key: 'home',
    },
    {
      label: <Link href='/registration'>Registration</Link>,
      key: 'registration',
    },
    {
      label: isLoggedIn ? (
        <Link href='/login' onClick={onLogout}>Logout</Link>
      ) : (
        <Link href='/login'>Login</Link>
      ),
      key: 'login',
    },
  ];

  return (
    <Layout>
      <Header>
        <Menu
          className='menu'
          theme='dark'
          mode='horizontal'
          items={navigationItems}
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
                />
              </Sider>
            </>
          )}
          <Content className='fit-content'>{children}</Content>
        </Layout>
      </Content>

      <Footer className='footer'>Chat Bot ©{new Date().getFullYear()}</Footer>
    </Layout>
  );
};

export default AppLayout;