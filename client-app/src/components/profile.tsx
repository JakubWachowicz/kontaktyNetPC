import React, { useState, useEffect } from 'react';
import { Box, Typography, TextField, Button, CircularProgress, List, ListItem, ListItemText, Dialog, DialogActions, DialogContent, DialogTitle } from '@mui/material';
import { useNavigate } from 'react-router-dom';
import agent from '../api/agent'; 

export interface UserProfileDto {
  id: number;
  firstName: string;
  lastName: string;
  dateOfBirth: string;
  userId: number;
}

export interface ContactDto {
  id: number;
  firstName: string | null;
  lastName: string | null;
  phoneNumber: string | null;
  contactEmail: string | null;
  contactDescription: string | null;
  categoryName: string | null
}

const ProfilePage: React.FC = () => {
  const [userProfile, setUserProfile] = useState<UserProfileDto | null>(null);
  const [contacts, setContacts] = useState<ContactDto[]>([]); // State for contacts
  const [loading, setLoading] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);
  const [editing, setEditing] = useState<boolean>(false);
  const [updatedProfile, setUpdatedProfile] = useState<UserProfileDto | null>(null);
  const [editingContact, setEditingContact] = useState<ContactDto | null>(null); // For editing a contact
  const [deletingContact, setDeletingContact] = useState<number>(-1); // For deleting a contact
  const [openDeleteDialog, setOpenDeleteDialog] = useState<boolean>(false); // Control Delete Dialog
  const navigate = useNavigate();

  // Fetch the user profile and their contacts when the component mounts
  useEffect(() => {
    const fetchUserProfile = async () => {
      try {
        const response = await agent.UserProfile.getUserProfile();
        setUserProfile(response);
        setUpdatedProfile(response);
        // Fetch the user's contacts
        const contactsResponse = await agent.UserProfile.getYourContacts();
        setContacts(contactsResponse);
      } catch (err) {
        setError('Error fetching profile or contacts');
        console.error(err);
      } finally {
        setLoading(false);
      }
    };

    fetchUserProfile();
  }, []);

  // Handle form field change for profile
  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    if (updatedProfile) {
      setUpdatedProfile({ ...updatedProfile, [name]: value });
    }
  };

  // Handle updating the profile
  const handleUpdate = async () => {
    if (updatedProfile) {
      try {
        await agent.UserProfile.updateUserProfile(updatedProfile); 
        setUserProfile(updatedProfile);
        setEditing(false);
      } catch (err) {
        setError('Error updating profile data');
        console.error(err);
      }
    }
  };

  // Handle adding a new contact (optional)
  const handleAddContact = () => {
    navigate('/profile/add-contact'); 
  };

  // Handle opening the delete dialog
  const handleDeleteDialogOpen = (id: number) => {
    setDeletingContact(id);
    setOpenDeleteDialog(true);
  };

  // Handle closing the delete dialog
  const handleDeleteDialogClose = () => {
    setOpenDeleteDialog(false);
    setDeletingContact(-1);
  };

  // Handle deleting a contact
  const handleDeleteContact = async () => {
    if (deletingContact) {
      try {
        await agent.Contacts.delete(deletingContact.toString()); 
        const updatedContacts = contacts.filter(contact => contact.id !== deletingContact);
        setContacts(updatedContacts);
        setOpenDeleteDialog(false);
        setDeletingContact(-1)
      } catch (err) {
        setError('Error deleting contact');
        console.error(err);
      }
    }
  };

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
      {userProfile ? (
        <>
          <Typography variant="h4" gutterBottom>
            Profile Information
          </Typography>
          {editing ? (
            // Editable profile form
            <Box>
              <TextField
                label="First Name"
                name="firstName"
                value={updatedProfile!.firstName}
                onChange={handleChange}
                fullWidth
                margin="normal"
              />
              <TextField
                label="Last Name"
                name="lastName"
                value={updatedProfile!.lastName}
                onChange={handleChange}
                fullWidth
                margin="normal"
              />
              <TextField
                label="Date of Birth"
                name="dateOfBirth"
                type="date"
                value={updatedProfile!.dateOfBirth}
                onChange={handleChange}
                fullWidth
                margin="normal"
                InputLabelProps={{
                  shrink: true,
                }}
              />
              <Box sx={{ marginTop: 2 }}>
                <Button variant="contained" onClick={handleUpdate}>Update Profile</Button>
                <Button variant="outlined" sx={{ marginLeft: 2 }} onClick={() => setEditing(false)}>Cancel</Button>
              </Box>
            </Box>
          ) : (
            // Display profile information
            <Box mb={2}>
              <Typography variant="h6">First Name: {userProfile.firstName}</Typography>
              <Typography variant="h6">Last Name: {userProfile.lastName}</Typography>
              <Typography variant="h6">Date of Birth: {new Date(userProfile.dateOfBirth).toLocaleDateString()}</Typography>
              <Box sx={{ marginTop: 2 }}>
                <Button variant="outlined" onClick={() => setEditing(true)}>Edit Profile</Button>
              </Box>
            </Box>
          )}

          {/* List of contacts */}
          <Box sx={{ marginTop: 4 }}>
            <Typography variant="h5" gutterBottom>
              Your Contacts
            </Typography>
            {contacts.length === 0 ? (
              <Typography>No contacts found</Typography>
            ) : (
              <List>
                {contacts.map((contact) => (
                  <ListItem key={contact.id}>
                    <ListItemText
                      primary={`${contact.firstName} ${contact.lastName}`}
                      secondary={`Phone: ${contact.phoneNumber}, Email: ${contact.contactEmail}, Category: ${contact.categoryName}`}
                    />
                    <Button onClick={() => navigate(`/profile/edit-contact/${contact.id}`)}>Edit</Button>

                    <Button color="error" onClick={() => handleDeleteDialogOpen(contact.id)}>Delete</Button>
                  </ListItem>
                ))}
              </List>
            )}
            <Box sx={{ marginTop: 2 }}>
              <Button variant="contained" onClick={handleAddContact}>Add New Contact</Button>
            </Box>
          </Box>
        </>
      ) : (
        <Typography variant="h6">Profile not found</Typography>
      )}

      {/* Delete Contact Dialog */}
      <Dialog open={openDeleteDialog} onClose={handleDeleteDialogClose}>
        <DialogTitle>Delete Contact</DialogTitle>
        <DialogContent>
          <Typography>Are you sure you want to delete this contact?</Typography>
        </DialogContent>
        <DialogActions>
          <Button onClick={handleDeleteDialogClose}>Cancel</Button>
          <Button onClick={handleDeleteContact} color="error">Delete</Button>
        </DialogActions>
      </Dialog>
    </Box>
  );
};

export default ProfilePage;
