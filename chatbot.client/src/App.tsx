import React from 'react';
import { BrowserRouter, Routes, Route } from 'react-router-dom';
import { Registration } from './pages/Registration';
import { Home } from './pages/Home';
import { Login } from './pages/Login';
import AppLayout from './components/layout/AppLayout';

const App: React.FC = () => {
  return (
    <BrowserRouter>
      <AppLayout>
        <Routes>
          <Route path="/" element={<Home />} />
          <Route path="registration" element={<Registration />} />
          <Route path="login" element={<Login />} />
        </Routes>
      </AppLayout>
    </BrowserRouter>
  );
};

export default App;