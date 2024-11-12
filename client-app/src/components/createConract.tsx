import React, { useState, useEffect } from 'react';
import { Box, Typography, TextField, Button, CircularProgress, MenuItem, Select, FormControl, InputLabel, SelectChangeEvent } from '@mui/material';
import { useNavigate } from 'react-router-dom';
import agent from '../api/agent';
import CategoryWithListDto from '../models/CategoryWithListsDto';

export interface CategoryDto {
  name: string;
  subcategoryName: string;
}

export interface CreateContactDto {
  ContactEmail: string;
  phoneNumber: string;
  contactDescription: string;
  category: CategoryDto;
}

const AddContactPage: React.FC = () => {
  const [newContact, setNewContact] = useState<CreateContactDto>({
    ContactEmail: '',
    phoneNumber: '',
    contactDescription: '',
    category: { name: '', subcategoryName: '' }, // Initialize category as an object
  });
  const [categories, setCategories] = useState<CategoryWithListDto[]>([]); // Store categories
  const [loading, setLoading] = useState<boolean>(false);
  const [error, setError] = useState<string | null>(null);
  const [subcategories, setSubcategories] = useState<string[]>([]); // To store subcategories of the selected category
  const navigate = useNavigate();

  // Fetch categories (simulate with mock data or fetch from API)
  useEffect(() => {
    const fetchCategories = async () => {
      try {
        let categoryList = await agent.Category.list();
        console.log("Fetched Categories:", categoryList); // Log fetched categories for debugging
        setCategories(categoryList); // Populate categories
      } catch (err) {
        setError('Error fetching categories');
        console.error(err);
      }
    };

    fetchCategories();
  }, []);

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
    const selectedCategoryId = e.target.value;
    console.log("Selected Category ID:", selectedCategoryId); // Log selected category for debugging

    const selectedCategory = categories.find((category) => category.name === selectedCategoryId);
    if (selectedCategory) {
      console.log("Selected Category:", selectedCategory); // Log selected category for debugging

      // Update the newContact state with the selected category
      setNewContact((prev) => ({
        ...prev,
        category: {
          name: selectedCategory.name,
          subcategoryName: '', // Clear subcategory when category changes
        },
      }));

      // Set the subcategories for the selected category
      setSubcategories(selectedCategory.SubcategoryName || []); // If there are no subcategories, set an empty array
    }
  };

  // Handle subcategory change (if applicable)
  const handleSubcategoryChange = (e: SelectChangeEvent<string>) => {
    setNewContact((prev) => ({
      ...prev,
      category: {
        ...prev.category,
        subcategoryName: e.target.value, // Update subcategory
      }
    }));
  };

  // Submit the form data
  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setLoading(true);
    try {
      await agent.Contacts.create(newContact); // Make the API call to create the new contact
      navigate('/profile'); // Redirect to the profile page after successful creation
    } catch (err) {
      setError('Error creating contact');
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  return (
    <Box sx={{ padding: 3 }}>
      <Typography variant="h4" gutterBottom>
        Add New Contact
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
                {category.name}
              </MenuItem>
            ))}
          </Select>
        </FormControl>

        {/* Subcategory Select (only show if the selected category has subcategories) */}
        {subcategories.length > 0 && (
          <FormControl fullWidth margin="normal">
            <InputLabel>Subcategory</InputLabel>
            <Select
              value={newContact.category.subcategoryName}
              onChange={handleSubcategoryChange}
              label="Subcategory"
            >
              {subcategories.map((subcategory, index) => (
                <MenuItem key={index} value={subcategory}>
                  {subcategory}
                </MenuItem>
              ))}
            </Select>
          </FormControl>
        )}

        <Box sx={{ marginTop: 2 }}>
          <Button variant="contained" color="primary" type="submit" disabled={loading}>
            {loading ? <CircularProgress size={24} /> : 'Create Contact'}
          </Button>
          <Button variant="outlined" sx={{ marginLeft: 2 }} onClick={() => navigate('/profile')}>
            Cancel
          </Button>
        </Box>
      </form>
    </Box>
  );
};

export default AddContactPage;
