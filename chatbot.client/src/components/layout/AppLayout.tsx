import { Layout, Menu } from 'antd';
import Link from 'antd/es/typography/Link';
import { ReactNode } from 'react';

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

      <Content className='content'>{children}</Content>

      <Footer className='footer'>Chat Bot ©{new Date().getFullYear()}</Footer>
    </Layout>
  );
};

export default AppLayout;