import React, { useState, useEffect } from 'react';
import { BrowserRouter as Router, Route, Routes, useNavigate } from 'react-router-dom';
import Navbar from './components/navbar';
import LoginForm from './components/loginForm';
import RegisterForm from './components/registerForm';
import { Container } from '@mui/material';
import agent from './api/agent';
import { User, Credentials } from './models/credentials';
import ProfilePage from './components/profile';
import HomePage from './components/home';
import ContactDetailsPage from './components/contactDetails';
import AddContactPage from './components/createConract';
import {EditContactPage} from './components/editContact'

const App: React.FC = () => {
  const [isAuthenticated, setIsAuthenticated] = useState<boolean>(false);
  const navigate = useNavigate(); 

  // Check if the user is logged in on initial render
  useEffect(() => {
    const token = localStorage.getItem('jwt');
    if (token) {
      setIsAuthenticated(true);
    }
  }, []);

  const login = async (credentials: Credentials) => {
    try {
      // Call login API
      const response = await agent.Account.login(credentials);
      const token = response;

      if (token) {
        // Store the token in localStorage
        localStorage.setItem('jwt', token.toString());
        setIsAuthenticated(true);
        console.log("JWT saved in localStorage:", token);

        // Navigate to the homepage after successful login
        navigate('/');
      } else {
        alert("Login failed. Please check your credentials.");
      }
    } catch (error) {
      console.error('Login failed:', error);
    }
  };

  const register = async (user: User) => {
    try {
      const response = await agent.Account.register(user);
      if (response.token) {
        localStorage.setItem('jwt', response.token); // Save token in local storage after registration
        setIsAuthenticated(true);
      }
    } catch (error) {
      console.error('Registration failed:', error);
    }
  };

  const logout = () => {
    localStorage.removeItem('jwt'); // Remove JWT from localStorage
    setIsAuthenticated(false); // Update the authentication state
    navigate('/login'); // Redirect to login page after logout
  };

  return (
   <>
      <Navbar isAuthenticated={isAuthenticated} onLogout={logout} />
      <Container sx={{ marginTop: 4 }}>
        <Routes>
          {/* Route for Login */}
          <Route path="/login" element={<LoginForm onLogin={login} />} />

          {/* Route for Register */}
          <Route path="/register" element={<RegisterForm onRegister={register} />} />

          {/* Route for Profile */}
          <Route path="/profile" element={<ProfilePage/>} />

          {/* Route for Contact details */}
          <Route path="/contact/:id" element={<ContactDetailsPage />} />
          {/* Route for add Contact */}
          <Route path="profile/add-contact" element={<AddContactPage />} />
           {/* Route for edit Contact */}
          <Route path="profile/edit-contact/:id" element={<EditContactPage />} />
          {/* Route for Homepage */}
          <Route path="/" element={<HomePage/>} />
        </Routes>
      </Container>
    </>
  );
};

export default App;
