import axios, { AxiosResponse } from 'axios';
import { AccesTokenService } from './AccessTokenService';
import { ValidateTokenModel } from '../models/ValidateTokenModel';
import { API_BASE_URL } from '../utils/Constants';
import { CustomAxiosRequestConfig } from '../models/CustomAxiosRequestConfigModel';

const apiClient = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    'Content-Type': 'application/json',
  }
});

apiClient.interceptors.request.use(async config => {
  let token = AccesTokenService.getAccessToken();
  let customConfig = config as CustomAxiosRequestConfig;

  if (!customConfig.skipAuthHeader) {
    if (AccesTokenService.isExpired()) {
      const oldAccessToken = AccesTokenService.getAccessToken();

      const validateToken: ValidateTokenModel = {
        accessToken: oldAccessToken!
      };

      const response = await post<string>('/api/tokens/refresh', validateToken, { skipAuthHeader: true });
      token = response;

      AccesTokenService.saveAccessToken(response);
    }
  }

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

async function get<T>(url: string, config?: CustomAxiosRequestConfig): Promise<T> {
  const response: AxiosResponse<T> = await apiClient.get(url, config);
  return response.data;
}

async function post<T, U = any>(url: string, data: U, config?: CustomAxiosRequestConfig): Promise<T> {
  const response: AxiosResponse<T> = await apiClient.post(url, data, config);
  return response.data;
}

async function put<T, U>(url: string, data: U, config?: CustomAxiosRequestConfig): Promise<T> {
  const response: AxiosResponse<T> = await apiClient.put(url, data, config);
  return response.data;
}

async function del<T>(url: string, config?: CustomAxiosRequestConfig): Promise<T> {
  const response: AxiosResponse<T> = await apiClient.delete(url, config);
  return response.data;
}

export { apiClient, get, post, put, del };