import { Typography } from 'antd';
import '../assets/styles/appLayout.css'
import { AccesTokenService } from '../services/AccessTokenService';
import { Navigate } from 'react-router-dom';

export const Home = () => {
  const { Title, Paragraph } = Typography;

  const isLoggedIn = AccesTokenService.isLoggedIn();

  if (!isLoggedIn) {
    return <Navigate to="/login" />;
  }
  
  return (
    <>
      <div className='text-align-center'>
        <Title>Welcome to the Home Page</Title>
        <Paragraph>This is the home page of our application.</Paragraph>
      </div>
    </>
  );
};