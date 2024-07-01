import { Typography } from 'antd';
import '../assets/styles/homePage.css'

export const Home = () => {
  const { Title, Paragraph} = Typography;

  return (
    <>
      <Title>Welcome to the Home Page</Title>
      <Paragraph>This is the home page of our application.</Paragraph>
    </>
  );
};