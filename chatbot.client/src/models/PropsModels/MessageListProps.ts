import { GetAllMessageModel } from "../GetAllMessageModel";

export interface MessageListProps {
    messages: GetAllMessageModel[];
    userId: string;
    isInChat: boolean;
    handleEmote: (emote: boolean | null, messageId: number) => void;
}