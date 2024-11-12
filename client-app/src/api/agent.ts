import axios, { AxiosResponse, AxiosError } from "axios";
import { Credentials, User } from "../models/credentials";
import { ContactDto } from "../models/contact";
import { UpdateUserProfileDto } from "../models/updateUserProfileDto";
import { CreateContactDto } from "../components/createConract";
import { UserProfileDto } from "../models/userProfileDto";
import CategoryWithListDto from "../models/CategoryWithListsDto";

axios.defaults.baseURL = "https://localhost:5001";

//JWT handling interceptor
axios.interceptors.request.use((config) => {
  const token = localStorage.getItem('jwt'); // Get token from localStorage
  if (token) {
    config.headers.Authorization = `Bearer ${token}`; // Attach JWT to header
  }
  return config;
}, (error) => {
  return Promise.reject(error);
});


//Error handling interceptor
axios.interceptors.response.use(
  async (response) => response,
  (error) => {
    console.error("API error:", error);
    return Promise.reject(error);
  }
);

//Response body model
const responseBody = <T>(response: AxiosResponse<T>) => response.data;
//Request model
const requests = {
  get: <T>(url: string) => axios.get<T>(url).then(responseBody),
  del: <T>(url: string) => axios.delete<T>(url).then(responseBody),
  put: <T>(url: string, body: {}) => axios.put<T>(url, body).then(responseBody),
  post: <T>(url: string, body: {}) => axios.post<T>(url, body).then(responseBody),
};

//Contact actions
const Contacts = {
  list: () => requests.get<ContactDto[]>("/Contact"),
  details: (id: string) => requests.get<ContactDto>(`/Contact/${id}`),
  create: (activity: CreateContactDto) => requests.post<CreateContactDto>("/Contact", activity),
  delete: (id: string) => requests.del<void>(`/Contact/${id}`),
  update: (id: string,activity: CreateContactDto,) => requests.put(`/Contact/${id}`,activity),
};
//User profile actions
const UserProfile = {
  getUserProfile: () => requests.get<UserProfileDto>("/UserProfile"),
  updateUserProfile: (updateUserProfileDto:UpdateUserProfileDto) => requests.put('/UserProfile',updateUserProfileDto),
  getYourContacts:() => requests.get<ContactDto[]>("/UserProfile/your-contacts")
}
//Account actions
const Account = {
  login: async (credentials: Credentials) => {
    const response = await axios.post<{ token: string }>('/Account/login', credentials);
    return response.data;
  },
  register: async (credentials: User) => {
    try {
      const response = await axios.post("/Account/register", credentials);
      return response.data;
    } catch (error) {
      throw error;
    }
  },
};

const Category = {
  list: () => requests.get<CategoryWithListDto[]>("/Category")
}

const agent = {
  Contacts,
  Account,
  UserProfile,
  Category
};
export default agent;
