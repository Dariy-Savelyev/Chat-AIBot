export interface MessageInputProps {
    isInChat: boolean;
    content: {
        content: string;
        chatId: number;
    };
    handleTextAreaChange: (e: React.ChangeEvent<HTMLTextAreaElement>) => void;
    submitContent: (chatId: number) => void;
    chatId: string | undefined;
}