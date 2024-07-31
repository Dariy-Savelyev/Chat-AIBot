import { Button, Input, Layout, Menu } from 'antd';
import Sider from 'antd/es/layout/Sider';
import Link from 'antd/es/typography/Link';
import { ChangeEvent, ReactNode, useCallback, useEffect, useState } from 'react';
import type { MenuProps } from 'antd';
import { ChatNameModel } from '../../models/ChatNameModel';
import { get, post } from '../../services/ApiClient';
import { Chat } from '../../models/GetAllChatModel';
import { useDispatch, useSelector } from 'react-redux';
import { setChats } from '../../store/slices/ChatSlice';
import { ChatStateModel } from '../../store/models/ChatStateModel';
import { useLocation } from 'react-router-dom';
import '../../assets/styles/appLayout.css';

const { Header, Content, Footer } = Layout;

interface AppLayoutProps {
  children: ReactNode;
}

const items = [
  {
    label: <Link href='/'>Home</Link>,
    key: 'home',
  },
  {
    label: <Link href='/registration'>Registration</Link>,
    key: 'registration',
  },
  {
    label: <Link href='/login'>Login</Link>,
    key: 'login',
  },
];

const AppLayout = ({ children }: AppLayoutProps) => {
  const [currentUrl, setCurrentUrl] = useState<ReactNode>();
  const [showInput, setShowInput] = useState(false);
  const [chatName, setFormData] = useState<ChatNameModel>({
    name: ''
  });
  
  const isHomePage = useLocation().pathname === '/';
  const chats = useSelector((state: ChatStateModel) => state.chats);

  const userChats: MenuProps['items'] = Object.values(chats).flat().map(
    (chat) => {
      return {
        key: chat.id,
        label: chat.name,
        onClick: () => {
          setCurrentUrl(<Link href={`/chat/${chat.id}`}>{`/chat/${chat.id}`}</Link>);
        },
      };
    },
  );

  const dispatch = useDispatch();
  
  const onReload = useCallback(async () => {
    const fetchedChats = await get<Chat[]>('/api/chat/getAllChats');

    dispatch(setChats(fetchedChats));
  }, [dispatch]);

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

      dispatch(setChats([{ id: +response, name: chatName.name }, ...Object.values(chats).flat()]));
    }
    finally {
      setFormData({ name: '' });
      setShowInput(false);
    }
  }, [chats, chatName, dispatch]);

  useEffect(() => {
    if (isHomePage) {
      onReload();
    }
  }, []);

  return (
    <Layout>
      <Header>
        <Menu
          className='menu'
          theme='dark'
          mode='horizontal'
          items={items}
        />
      </Header>

      <Content>
        <Layout>
          {isHomePage && (
            <>
              <Sider>
                {showInput ? (
                  <>
                    <Input
                      className='input'
                      type='text'
                      value={chatName.name}
                      onChange={handleChange}
                      placeholder='Enter chat name'
                    />
                    <Button className='button-input' type='primary' htmlType='submit' onClick={submitChatName}>Create</Button>
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
          <Content className='fit-content'> {currentUrl ? (
            <div className='text-align-center'>{currentUrl}</div>
          ) : (
            children
          )}
          </Content>
        </Layout>
      </Content>

      <Footer className='footer'>Chat Bot ©{new Date().getFullYear()}</Footer>
    </Layout>
  );
};

export default AppLayout;