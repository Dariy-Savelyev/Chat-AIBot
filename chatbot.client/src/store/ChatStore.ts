import { configureStore } from '@reduxjs/toolkit';
import chatReducer from './slices/ChatSlice';

export default configureStore({
  reducer: {
    chats: chatReducer,
  },
});