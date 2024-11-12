import React, { useEffect, useState } from 'react';
import { Box, Typography, Button, CircularProgress, Paper, IconButton } from '@mui/material';
import { useParams, useNavigate } from 'react-router-dom';
import DeleteIcon from '@mui/icons-material/Delete';
import { ContactDto } from '../models/contact';
import agent from '../api/agent';

const ContactDetailsPage: React.FC = () => {
  const [contact, setContact] = useState<ContactDto | null>(null);
  const [loading, setLoading] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);
  const { id } = useParams<{ id: string }>(); //Get id from url
  const navigate = useNavigate();

  // Fetch the contact details on component mount
  useEffect(() => {
    const fetchContactDetails = async () => {
      try {
        const contactDetails = await agent.Contacts.details(id!); 
        setContact(contactDetails);
      } catch (err) {
        setError('Error fetching contact details');
        console.error(err);
      } finally {
        setLoading(false);
      }
    };

    if (id) {
      fetchContactDetails();
    }
  }, [id]);



  // Loading and error states
  if (loading) {
    return (
      <Box display="flex" justifyContent="center" alignItems="center" height="100vh">
        <CircularProgress />
      </Box>
    );
  }

  if (error) {
    return (
      <Box display="flex" justifyContent="center" alignItems="center" height="100vh">
        <Typography variant="h6" color="error">{error}</Typography>
      </Box>
    );
  }

  if (!contact) {
    return (
      <Box display="flex" justifyContent="center" alignItems="center" height="100vh">
        <Typography variant="h6" color="error">Contact not found.</Typography>
      </Box>
    );
  }

  return (
    <Box sx={{ padding: 3 }}>
      <Paper sx={{ padding: 3 }}>
        <Typography variant="h4" gutterBottom>
          {`${contact.firstName} ${contact.lastName}`}
        </Typography>

        <Typography variant="h6" gutterBottom>
          Email: {contact.contactEmail}
        </Typography>

        <Typography variant="h6" gutterBottom>
          Phone: {contact.phoneNumber}
        </Typography>

        <Typography variant="h6" gutterBottom>
          Description: {contact.contactDescription}
        </Typography>

      </Paper>
    </Box>
  );
};

export default ContactDetailsPage;
