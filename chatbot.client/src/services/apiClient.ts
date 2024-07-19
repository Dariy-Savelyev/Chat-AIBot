import axios from 'axios';

const apiClient = axios.create({
  baseURL: import.meta.env.VITE_API_BASE_URL,
  headers: {
    'Content-Type': 'application/json',
  }
});

// Добавление интерсептора для добавления токена авторизации перед каждым запросом
apiClient.interceptors.request.use(config => {
  const token = localStorage.getItem('accessToken'); // Предположим, что токен хранится в localStorage
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
}, error => {
  return Promise.reject(error);
});

apiClient.interceptors.response.use(response => {
  return response;
}, error => {
  if (error.response && error.response.status === 401) {
    window.location.href = '/login';
  }
  return Promise.reject(error);
});

export default apiClient;