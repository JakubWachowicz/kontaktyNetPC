import React, { useState, useEffect } from 'react';
import { Box, Typography, TextField, Button, CircularProgress, MenuItem, Select, FormControl, InputLabel, SelectChangeEvent } from '@mui/material';
import { useNavigate, useParams } from 'react-router-dom';
import agent from '../api/agent'; 
import { CategoryDto, CreateContactDto } from './createConract';
import CategoryWithListDto from '../models/CategoryWithListsDto';

export const EditContactPage: React.FC = () => {
  const { id } = useParams<{ id: string }>(); 
  const [newContact, setNewContact] = useState<CreateContactDto>({
    ContactEmail: '',
    phoneNumber: '',
    contactDescription: '',
    category: { name: "", subcategoryName: '' }, // Initialize category as an object
  });
  const [categories, setCategories] = useState<CategoryWithListDto[]>([]); // Store categories
  const [loading, setLoading] = useState<boolean>(false);
  const [error, setError] = useState<string | null>(null);
  const navigate = useNavigate();

  // Fetch contact and categories
  useEffect(() => {

    const fetchContact = async () => {
      try {
        const contact = await agent.Contacts.details(id!.toString());
        const createContactDto: CreateContactDto = {
          ContactEmail: contact.contactEmail || "",
          phoneNumber: contact.phoneNumber || "",
          contactDescription: contact.contactDescription || "",
          category: { name: contact.categoryName || "", subcategoryName: contact.subCategory || "" },
        };
        setNewContact(createContactDto);
      } catch (err) {
        setError('Error fetching contact');
        console.error(err);
      }
    };

    const fetchCategories = async () => {
      try {
        let categoryList = await agent.Category.list();
        setCategories(categoryList); // Populate categories
      } catch (err) {
        setError('Error fetching categories');
        console.error(err);
      }
    };

    fetchCategories();
    fetchContact();
  }, [id]);

  // Handle changes for text fields and select dropdown
  const handleChange = (e: React.ChangeEvent<HTMLInputElement | { name?: string; value: unknown }>) => {
    const { name, value } = e.target;
    setNewContact((prev) => ({
      ...prev,
      [name as string]: value,
    }));
  };

  // Handle changes for Select field (category dropdown)
  const handleSelectChange = (e: SelectChangeEvent<string>) => {
    const selectedCategoryName = e.target.value;
    const selectedCategory = categories.find((category) => category.name === selectedCategoryName);

    if (selectedCategory) {
      setNewContact((prev) => ({
        ...prev,
        category: {
          name: selectedCategory.name,
          subcategoryName: selectedCategory?.SubcategoryName![0] || "", // Default to first subcategory
        },
      }));
    }
  };

  // Handle changes for subcategory if it's editable
  const handleSubcategoryChange = (e: SelectChangeEvent<string>) => {
    const selectedSubcategory = e.target.value;
    setNewContact((prev) => ({
      ...prev,
      category: {
        ...prev.category,
        subcategoryName: selectedSubcategory,
      }
    }));
  };

  // Submit the form data
  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setLoading(true);
    try {
      await agent.Contacts.update(id!, newContact); // Update contact API call
      navigate('/profile'); // Redirect to profile after successful edit
    } catch (err) {
      setError('Error updating contact');
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  return (
    <Box sx={{ padding: 3 }}>
      <Typography variant="h4" gutterBottom>
        Edit Contact
      </Typography>

      {error && (
        <Typography variant="h6" color="error" gutterBottom>
          {error}
        </Typography>
      )}

      <form onSubmit={handleSubmit}>
        <TextField
          label="Email"
          name="ContactEmail"
          type="email"
          value={newContact.ContactEmail}
          onChange={handleChange}
          fullWidth
          margin="normal"
          required
        />
        <TextField
          label="Phone Number"
          name="phoneNumber"
          value={newContact.phoneNumber}
          onChange={handleChange}
          fullWidth
          margin="normal"
          type="tel"
        />
        <TextField
          label="Contact Description"
          name="contactDescription"
          value={newContact.contactDescription}
          onChange={handleChange}
          fullWidth
          margin="normal"
          multiline
          rows={4}
        />

        {/* Category Select */}
        <FormControl fullWidth margin="normal" required>
          <InputLabel>Category</InputLabel>
          <Select
            name="category"
            value={newContact.category.name}
            onChange={handleSelectChange} // Handle category selection
            label="Category"
          >
            {categories.map((category) => (
              <MenuItem key={category.name} value={category.name}>
                {category.name} {/* Display category name in the dropdown */}
              </MenuItem>
            ))}
          </Select>
        </FormControl>

       
        <Box sx={{ marginTop: 2 }}>
          <Button variant="contained" color="primary" type="submit" disabled={loading}>
            {loading ? <CircularProgress size={24} /> : 'Edit Contact'}
          </Button>
          <Button variant="outlined" sx={{ marginLeft: 2 }} onClick={() => navigate('/profile')}>
            Cancel
          </Button>
        </Box>
      </form>
    </Box>
  );
};

export default EditContactPage;
