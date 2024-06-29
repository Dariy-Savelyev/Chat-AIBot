import { Link } from 'react-router-dom';

export const Home = () => {
  function Nav() {
    return <nav>
      <div>
        <Link to="/registration">Registration</Link>
        <Link to="/login">Login</Link>
      </div>
    </nav>;
  }

  return (
    <>
      <Nav />
      <div className="home">
        <h1>Welcome to the Home Page</h1>
        <p>This is the home page of our application.</p>
      </div >
    </>
  );
};