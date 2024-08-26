import { ChangeEvent, useCallback, useContext, useEffect, useState } from "react";
import { MessageModel } from "../models/MessageModel";
import { get, post } from "../services/ApiClient";
import { useParams } from "react-router-dom";
import '../assets/styles/chat.css';
import { GetAllMessageModel } from "../models/GetAllMessageModel";
import { useDispatch, useSelector } from "react-redux";
import { addMessage, setMessages } from "../store/slices/MessageSlice";
import { MessageStateModel } from "../store/models/MessageStateModel";
import { EmoteModel } from "../models/EmoteModel";
import { ChatStateModel } from "../store/models/ChatStateModel";
import { MessageList } from "../components/chat/MessageList";
import { MessageForm } from "../components/chat/MessageForm";
import { UserContext } from "../components/layout/AppLayout";
import { Spin } from "antd";
import { LoadingOutlined } from '@ant-design/icons';
import { hubService } from "../services/HubService";
import { HubMessageModel } from "../models/HubMessageModel";

const loadingIcon = <LoadingOutlined style={{ fontSize: 60 }} spin />;

export const SelectedChat = () => {
    const [isInChat, setIsInChat] = useState(false);
    const [isLoading, setIsLoading] = useState(false);

    const [content, setContent] = useState<MessageModel>({
        content: '',
        chatId: 0
    });

    const emoteModel: EmoteModel = {
        emote: null,
        messageId: 0
    };

    const userId = useContext(UserContext);
    const chats = useSelector((state: ChatStateModel) => state.chats);
    const messages = useSelector((state: MessageStateModel) => state.messages);
    const { chatId } = useParams();

    const dispatch = useDispatch();

    const onReload = useCallback(async () => {
        setIsLoading(true);
        try {
            const fetchedMessages = await get<GetAllMessageModel[]>(`/api/message/getAllMessages?chatId=${chatId}`);
            dispatch(setMessages(fetchedMessages));
        } finally {
            setIsLoading(false);
        }
    }, [dispatch, chatId]);

    const isUserInChat = useCallback(() => {
        const userInChat = Object.values(chats).flat().find(chat =>
            chat.id === Number(chatId) && chat.userIds.includes(userId)
        );

        setIsInChat(!!userInChat);
    }, [chats, chatId, userId]);

    const submitContent = useCallback(async (chatId: number) => {
        try {
            content.chatId = chatId;

            const response = await post<string>('/api/message/send', content);

            if (response != '') {
                dispatch(addMessage({
                    content: content.content,
                    id: +response,
                    emote: null,
                    userId: userId
                }));
            }
        }
        finally {
            setContent((prevContent) => ({ ...prevContent, content: '' }));
        }
    }, [content, dispatch, chatId]);

    const handleTextAreaChange = useCallback((e: ChangeEvent<HTMLTextAreaElement>) => {
        setContent(prevContent => ({
            ...prevContent,
            content: e.target.value
        }));
    }, []);

    const handleEmote = useCallback(async (emote: boolean | null, messageId: number) => {
        emoteModel.emote = emote;
        emoteModel.messageId = messageId;

        await post<string>('/api/message/setEmote', emoteModel);

        dispatch(setMessages(Object.values(messages).flat().map((message) => (
            message.id === messageId ? { ...message, emote: emote } : message
        ))));
    }, [emoteModel, dispatch, messages]);

    const messageListener = useCallback((message: HubMessageModel) => {
        if (userId !== message.userId && chatId === message.chatId.toString()) {
            dispatch(addMessage({
                content: message.content,
                id: message.id,
                emote: null,
                userId: message.userId
            }));
        }
    }, [chatId, dispatch]);

    useEffect(() => {
        hubService.startConnection('https://localhost:7286/chatHub');
        hubService.addMessageListener(messageListener);

        return () => {
            hubService.removeMessageListener();
        };
    }, [chatId]);

    useEffect(() => {
        isUserInChat();

        dispatch(setMessages([]));

        onReload();

        setContent((prevContent) => ({ ...prevContent, content: '' }));
    }, [isUserInChat, onReload, chatId]);

    if (isLoading) {
        return (
            <div className='spin-container'>
                <Spin size='large' indicator={loadingIcon} />
            </div>
        );
    }

    return (
        <>
            <MessageList
                messages={messages}
                userId={userId}
                isInChat={isInChat}
                handleEmote={handleEmote}
            />
            <MessageForm
                isInChat={isInChat}
                content={content}
                handleTextAreaChange={handleTextAreaChange}
                submitContent={submitContent}
                chatId={chatId}
            />
        </>
    );
};