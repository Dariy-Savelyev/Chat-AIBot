import { createSelector } from 'reselect';
import { ChatStateModel } from '../models/ChatStateModel';
import { GetAllChatModel } from '../../models/GetAllChatModel';

const getChats = (state: ChatStateModel) => state.chats;
const getUserId = (_state: ChatStateModel, userId: string) => userId;

export const getChatsWithJoinedStatus = createSelector(
  [getChats, getUserId],
  (chats, userId): GetAllChatModel[] => {
    return Object.values(chats).flat().map(chat => ({
      ...chat,
      joined: chat.userIds.includes(userId)
    }));
  }
);