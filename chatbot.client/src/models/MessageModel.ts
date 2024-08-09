export interface MessageModel {
    id: number;
    content: string;
    chatId: number;
    emote: boolean | null;
}