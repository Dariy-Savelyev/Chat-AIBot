import { Typography } from 'antd';

export const Home = () => {
  function Nav() {
    return <div style={{ position: 'relative', top: 60, left: '47%', fontWeight: 'bold' }}>
      <Typography.Link href='/registration'>Registration</Typography.Link>
      <Typography.Link href='/login'
        style={{ position: 'relative', left: 10 }}>Login</Typography.Link>
    </div>;
  }

  return (
    <>
      <Nav />
      <div style={{ position: 'relative', top: 70, left: '38%' }}>
        <Typography.Title>Welcome to the Home Page</Typography.Title>
        <Typography.Paragraph>This is the home page of our application.</Typography.Paragraph>
      </div >
    </>
  );
};