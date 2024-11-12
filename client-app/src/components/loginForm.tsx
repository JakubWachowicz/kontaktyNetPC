import React, { useState } from 'react';
import { TextField, Button, Box, Typography } from '@mui/material';
import { Credentials } from '../models/credentials';

interface LoginFormProps {
  onLogin: (credentials:Credentials) => void;
}

const LoginForm: React.FC<LoginFormProps> = ({ onLogin }) => {
  const [email, setEmail] = useState<string>('');
  const [password, setPassword] = useState<string>('');

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    var credentials: Credentials = {
      email:email,
      password:password
    }
    onLogin(credentials);
  };

  return (
    <Box
      sx={{
        display: 'flex',
        flexDirection: 'column',
        alignItems: 'center',
        maxWidth: 400,
        margin: 'auto',
        padding: 3,
        boxShadow: 3,
        borderRadius: 2,
      }}
    >
      <Typography variant="h5" sx={{ marginBottom: 2 }}>
        Login
      </Typography>
      <form onSubmit={handleSubmit} style={{ width: '100%' }}>
        <TextField
          label="Email"
          type="email"
          fullWidth
          margin="normal"
          value={email}
          onChange={(e) => setEmail(e.target.value)}
          required
        />
        <TextField
          label="Password"
          type="password"
          fullWidth
          margin="normal"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
          required
        />
        <Button type="submit" variant="contained" fullWidth sx={{ marginTop: 2 }}>
          Login
        </Button>
      </form>
    </Box>
  );
};

export default LoginForm;
