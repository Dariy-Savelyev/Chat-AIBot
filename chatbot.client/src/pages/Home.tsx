import { Layout, Menu, Typography } from 'antd';
import { Content, Footer, Header } from 'antd/es/layout/layout';
import '../assets/styles/homePage.css'

export const Home = () => {
  const { Title, Paragraph, Link } = Typography;

  const items = [
    {
      label: <Link href='/registration'>Registration</Link>,
      key: 'registration',
    },
    {
      label: <Link href='/login'>Login</Link>,
      key: 'login',
    },
  ];

  return (
    <>
      <Layout>
        <Header>
          <Menu
            className='menu'
            theme='dark'
            mode='horizontal'
            items={items}
          />
        </Header>

        <Content className='content'>
          <Title>Welcome to the Home Page</Title>
          <Paragraph>This is the home page of our application.</Paragraph>
        </Content>

        <Footer className='footer'>
          Chat Bot ©{new Date().getFullYear()}
        </Footer>
      </Layout>
    </>
  );
};