import { createSlice, PayloadAction } from '@reduxjs/toolkit';
import { GetAllMessageModel } from '../../models/GetAllMessageModel';

export const messageSlice = createSlice({
  name: 'messages',
  initialState: [] as GetAllMessageModel[],
  reducers: {
    setMessages: (_state, action: PayloadAction<GetAllMessageModel[]>) => {
      return action.payload;
    },
    addMessage: (state, action: PayloadAction<GetAllMessageModel>) => {
      state.push(action.payload);
    },
  },
});

export const { setMessages, addMessage } = messageSlice.actions;
export default messageSlice.reducer;