import { Button, Input, Layout, Menu } from 'antd';
import Sider from 'antd/es/layout/Sider';
import Link from 'antd/es/typography/Link';
import React, { ChangeEvent, ReactNode, useCallback, useState } from 'react';
import { LaptopOutlined, NotificationOutlined, UserOutlined } from '@ant-design/icons';
import type { MenuProps } from 'antd';
import { ChatNameModel } from '../../models/ChatNameModel';
import { post } from '../../services/ApiClient';

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

const items2: MenuProps['items'] = [UserOutlined, LaptopOutlined, NotificationOutlined].map(
  (icon, index) => {
    const key = String(index + 1);

    return {
      key: `sub${key}`,
      icon: React.createElement(icon),
      label: `subnav ${key}`,

      children: new Array(4).fill(null).map((_, j) => {
        const subKey = index * 4 + j + 1;
        return {
          key: subKey,
          label: `option${subKey}`,
        };
      }),
    };
  },
);

const AppLayout = ({ children }: AppLayoutProps) => {
  const [showInput, setShowInput] = useState(false);
  const [chatName, setFormData] = useState<ChatNameModel>({
    name: ''
  });

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
      await post<string>('/api/chat/create', chatName, { skipAuthHeader: true });
    }
    finally {
      setShowInput(false);
    }
  }, [chatName]);

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
          <Sider>
            {showInput ? (
              <>
                <Input
                  className='input-width'
                  type='text'
                  value={chatName.name}
                  onChange={handleChange}
                  placeholder='Enter chat name'
                />
                <Button type='primary' htmlType='submit' onClick={submitChatName}>Create</Button>
              </>
            ) : (
              <Button className='button-width' type='primary' onClick={showChatInput}>Create Chat</Button>
            )}
            <Menu
              className='menu-height'
              mode='inline'
              items={items2}
            />
          </Sider>
          <Content>{children}</Content>
        </Layout>
      </Content>

      <Footer className='footer'>Chat Bot ©{new Date().getFullYear()}</Footer>
    </Layout>
  );
};

export default AppLayout;