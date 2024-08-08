import { configureStore } from '@reduxjs/toolkit';
import chatReducer from './slices/ChatSlice';
import messagesReducer from './slices/MessageSlice';

export default configureStore({
  reducer: {
    chats: chatReducer,
    messages: messagesReducer,
  },
});