import { Typography } from 'antd';
import '../assets/styles/appLayout.css'

export const Home = () => {
  const { Title, Paragraph } = Typography;

  return (
    <>
      <div className='text-align-center'>
        <Title> Welcome to the Home Page</Title>
        <Paragraph>This is the home page of our application.</Paragraph>
      </div>
    </>
  );
};