import { createSlice, PayloadAction } from '@reduxjs/toolkit';
import { MessageStateModel } from '../models/MessageStateModel';
import { GetAllMessageModel } from '../../models/GetAllMessageModel';

const initialState: MessageStateModel = {
  messages: [],
};

export const messageSlice = createSlice({
  name: 'messages',
  initialState,
  reducers: {
    setMessages: (state, action: PayloadAction<GetAllMessageModel[]>) => {
      state.messages = action.payload;
    },
  },
});

export const { setMessages } = messageSlice.actions;
export default messageSlice.reducer;