export interface MessageItemProps {
    item: {
        id: number;
        userId: string;
        content: string;
        emote: boolean | null;
    };
    userId: string;
    isInChat: boolean;
    handleEmote: (emote: boolean | null, messageId: number) => void;
}