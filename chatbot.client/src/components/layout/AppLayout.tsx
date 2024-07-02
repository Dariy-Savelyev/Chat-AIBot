import { Layout, Menu } from 'antd';
import Sider from 'antd/es/layout/Sider';
import Link from 'antd/es/typography/Link';
import React, { ReactNode } from 'react';
import { LaptopOutlined, NotificationOutlined, UserOutlined } from '@ant-design/icons';
import type { MenuProps } from 'antd';

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
          <Sider >
            <Menu
              className='menu-height'
              mode="inline"
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