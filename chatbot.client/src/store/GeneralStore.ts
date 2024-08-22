import { configureStore } from '@reduxjs/toolkit';
import chatReducer from './slices/ChatSlice';
import appReducer from './slices/AppSlice';
import messagesReducer from './slices/MessageSlice';

export default configureStore({
  reducer: {
    chats: chatReducer,
    messages: messagesReducer,
    app: appReducer,
  },
});