import React, { useEffect, useState } from 'react';
import { Box, Typography, Button, Grid, Card, CardContent, CardActions, CircularProgress, IconButton } from '@mui/material';
import { Link } from 'react-router-dom'; 
import { ContactDto } from '../models/contact';
import agent from '../api/agent';

const HomePage: React.FC = () => {
  const [contacts, setContacts] = useState<ContactDto[]>([]);
  const [loading, setLoading] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);

  // Fetch the list of contacts on component mount
  useEffect(() => {
    const fetchContacts = async () => {
      try {
        const contactList = await agent.Contacts.list();
        setContacts(contactList);
      } catch (err) {
        setError('Error fetching contacts');
        console.error(err);
      } finally {
        setLoading(false);
      }
    };

    fetchContacts();
  }, []);


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

  return (
    <Box sx={{ padding: 3 }}>
      <Typography variant="h4" gutterBottom>Contact List</Typography>

      {/* Contact List - Cards Layout */}
      <Grid container spacing={3}>
        {contacts.map(contact => (
          <Grid item xs={12} sm={6} md={4} key={contact.id}>
            <Card sx={{ height: '100%' }}>
              <CardContent>
                <Typography variant="h6">{`${contact.firstName} ${contact.lastName}`}</Typography>
                <Typography variant="body2" color="textSecondary">{`Email: ${contact.contactEmail}`}</Typography>
                <Typography variant="body2" color="textSecondary">{`Phone: ${contact.phoneNumber}`}</Typography>
                <Typography variant="body2" color="textSecondary">{`Category: ${contact.categoryName}`}</Typography>
                <Typography variant="body2" color="textSecondary">{`SubCategory: ${contact.subCategory}`}</Typography>
              </CardContent>
              <CardActions sx={{ justifyContent: 'space-between' }}>
                <Button
                  variant="outlined"
                  color="primary"
                  component={Link}
                  to={`/contact/${contact.id}`}
                >
                  View
                </Button>
             
              </CardActions>
            </Card>
          </Grid>
        ))}
      </Grid>
    </Box>
  );
};

export default HomePage;
