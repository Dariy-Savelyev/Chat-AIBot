import { Button, Card, ConfigProvider, Input, List, Space, Typography } from "antd";
import { ChangeEvent, useCallback, useEffect, useState } from "react";
import { MessageModel } from "../models/MessageModel";
import { get, post } from "../services/ApiClient";
import { useParams } from "react-router-dom";
import '../assets/styles/chat.css';
import { GetAllMessageModel } from "../models/GetAllMessageModel";
import { useDispatch, useSelector } from "react-redux";
import { setMessages } from "../store/slices/MessageSlice";
import { MessageStateModel } from "../store/models/MessageStateModel";
import likeIcon from '../assets/images/likeIcon.png';
import dislikeIcon from '../assets/images/dislikeIcon.png';
import { EmoteModel } from "../models/EmoteModel";

export const SelectedChat = () => {
    const [isInChat, setIsInChat] = useState(false);
    const [userId, setUserId] = useState('');

    const [content, setContent] = useState<MessageModel>({
        content: '',
        chatId: 0
    });

    const emoteModel: EmoteModel = {
        emote: null,
        messageId: 0
    };

    const messages = useSelector((state: MessageStateModel) => state.messages);
    const { chatId } = useParams();

    const dispatch = useDispatch();

    const onReload = useCallback(async () => {
        const userId = await get<string>('/api/account/userInfo');
        setUserId(userId);

        const fetchedMessages = await get<GetAllMessageModel[]>(`/api/message/getAllMessages?chatId=${chatId}`);

        dispatch(setMessages(fetchedMessages));
    }, [dispatch]);

    const isUserInChat = useCallback(async () => {
        const isUserInChat = await get<boolean>(`/api/chat/isUserJoined?chatId=${chatId}`);

        setIsInChat(isUserInChat);
    }, []);

    const submitContent = useCallback(async (chatId: number) => {
        try {
            content.chatId = chatId;

            const messageId = await post<string>('/api/message/send', content);

            dispatch(setMessages([...Object.values(messages).flat(),
            { content: content.content, id: +messageId, emote: null, userId: userId }]));
        }
        finally {
            setContent((prevContent) => ({ ...prevContent, content: '' }));
        }
    }, [content, dispatch, messages]);

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

    useEffect(() => {
        isUserInChat();

        onReload();
    }, []);

    return (
        <>
            <div className="chat-scrollbar">
                <List
                    itemLayout="horizontal"
                    locale={{ emptyText: true }}
                    dataSource={Object.values(messages).flat()}
                    split={false}
                    renderItem={(item) => (
                        <List.Item className={`message-display ${item.userId === userId ? 'user-display' : 'other-user-display'}`}>
                            <Card
                                className="chat-bubble"
                                onMouseEnter={(e) => {
                                    if (isInChat) {
                                        e.currentTarget.classList.add("show-reactions");
                                    }
                                }}
                                onMouseLeave={(e) => {
                                    e.currentTarget.classList.remove("show-reactions");
                                }}
                            >
                                <Typography.Text className="text-content-style">{item.content}</Typography.Text>
                                <ConfigProvider wave={{ disabled: true }}>
                                    {item.emote !== null ? (
                                        <Button
                                            className="button-emote button-position"
                                            ghost
                                            size="small"
                                            type="primary"
                                            icon={item.emote ? (
                                                <img src={likeIcon} alt="unLike" className="icon-small" />
                                            ) : (
                                                <img src={dislikeIcon} alt="dislike" className="icon-small" />
                                            )}
                                            onClick={() => handleEmote(null, item.id)}
                                            disabled={!isInChat}
                                        />
                                    ) : null}
                                </ConfigProvider>
                                {item.emote == null ? (
                                    <div className="reactions">
                                        <ConfigProvider wave={{ disabled: true }}>
                                            <Button
                                                className="button-emote"
                                                ghost
                                                size="small"
                                                type="primary"
                                                icon={<img src={likeIcon} alt="like" className="icon-small" />}
                                                onClick={() => handleEmote(true, item.id)}
                                            />
                                            <span className="divider-color">│</span>
                                            <Button
                                                style={{ outline: 'none', boxShadow: 'none' }}
                                                className="button-emote"
                                                ghost
                                                size="small"
                                                type="primary"
                                                icon={<img src={dislikeIcon} alt="dislike" className="icon-small" />}
                                                onClick={() => handleEmote(false, item.id)}
                                            />
                                        </ConfigProvider>
                                    </div>
                                ) : null}
                            </Card>
                        </List.Item>
                    )}
                />
            </div>
            <Space.Compact className="space-compact-position">
                <Input.TextArea
                    className='textarea'
                    disabled={!isInChat}
                    autoSize={{ minRows: 1, maxRows: 1 }}
                    value={content.content}
                    onChange={handleTextAreaChange}
                    placeholder="Write message..."
                    onKeyDown={(event) => {
                        if (event.key === 'Enter' && content.content.trim() !== '') {
                            event.preventDefault();
                            submitContent(Number(chatId));
                        }
                    }}
                />
                <Button
                    className='button-chat'
                    type='primary'
                    htmlType='submit'
                    onClick={() => submitContent(Number(chatId))}
                    disabled={content.content.trim() === ''}
                >
                    Send
                </Button>
            </Space.Compact>
        </>
    );
};