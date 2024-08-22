import { createSlice, PayloadAction } from '@reduxjs/toolkit';
import { AppStateModel } from '../models/AppStateModel';

const initialState: AppStateModel = {
  Version: "N/A",
};

export const appSlice = createSlice({
  name: 'app',
  initialState,
  reducers: {
    setApp: (state, action: PayloadAction<string>) => {
      state.Version = action.payload;
    },
  },
});

export const { setApp } = appSlice.actions;
export default appSlice.reducer;