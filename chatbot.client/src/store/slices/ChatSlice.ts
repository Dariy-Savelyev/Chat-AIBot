import { createSlice, PayloadAction } from '@reduxjs/toolkit';
import { GetAllChatModel } from '../../models/GetAllChatModel';
import { ChatStateModel } from '../models/ChatStateModel';

const initialState: ChatStateModel = {
  chats: [],
};

export const chatSlice = createSlice({
  name: 'chats',
  initialState,
  reducers: {
    setChats: (state, action: PayloadAction<GetAllChatModel[]>) => {
      state.chats = action.payload;
    },
  },
});

export const { setChats } = chatSlice.actions;
export default chatSlice.reducer;