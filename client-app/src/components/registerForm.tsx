import React, { useState } from 'react';
import { TextField, Button, Box } from '@mui/material';
import { DatePicker, LocalizationProvider } from '@mui/x-date-pickers';
import dayjs, { Dayjs } from 'dayjs';
import { AdapterDayjs } from '@mui/x-date-pickers/AdapterDayjs';
import { User } from '../models/credentials';


interface RegisterFormProps {
  onRegister: (
    user:User
  ) => void;
}

const RegisterForm: React.FC<RegisterFormProps> = ({ onRegister }) => {
  const [firstName, setFirstName] = useState<string>('');
  const [lastName, setLastName] = useState<string>('');
  const [email, setEmail] = useState<string>('');
  const [password, setPassword] = useState<string>('');
  const [dateOfBirth, setDateOfBirth] = useState<Dayjs | null>(null);

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    if (dateOfBirth) {
      var user:User ={
        firstName:firstName,
        lastName:lastName,
        email:email,
        password:password,
        dateOfBirth:dateOfBirth.format('YYYY-MM-DD'),

      }
      onRegister(user);
    }
  };

  return (
    <form onSubmit={handleSubmit}>
      <Box sx={{ display: 'flex', flexDirection: 'column', gap: 2 }}>
        <TextField
          label="First Name"
          variant="outlined"
          value={firstName}
          onChange={(e) => setFirstName(e.target.value)}
          required
        />
        <TextField
          label="Last Name"
          variant="outlined"
          value={lastName}
          onChange={(e) => setLastName(e.target.value)}
          required
        />
        <TextField
          label="Email"
          variant="outlined"
          value={email}
          onChange={(e) => setEmail(e.target.value)}
          required
        />
        <TextField
          label="Password"
          variant="outlined"
          type="password"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
          required
        />
       <LocalizationProvider dateAdapter={AdapterDayjs}>
            <DatePicker label="Basic date picker"  onChange={(newValue: Dayjs | null) => setDateOfBirth(newValue)}/>
        </LocalizationProvider>
        <Button type="submit" variant="contained">
          Register
        </Button>
      </Box>
    </form>
  );
};

export default RegisterForm;
