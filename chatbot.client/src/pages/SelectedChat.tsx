import { Button, Card, ConfigProvider, Input, List, Space, Typography } from "antd";
import { ChangeEvent, useCallback, useEffect, useRef, useState } from "react";
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
    const [content, setContent] = useState<MessageModel>({
        id: 0,
        content: '',
        chatId: 0,
        emote: null
    });

    const emoteModel: EmoteModel = {
        emote: null,
        messageId: 0
    };

    const messages = useSelector((state: MessageStateModel) => state.messages);
    const { chatId } = useParams();
    const contentRef = useRef(content);

    const dispatch = useDispatch();

    const onReload = useCallback(async () => {
        const fetchedMessages = await get<GetAllMessageModel[]>(`/api/message/getAllMessages?chatId=${chatId}`);

        dispatch(setMessages(fetchedMessages));
    }, [dispatch]);

    useEffect(() => {
        if (messages != null) {
            onReload();
        }
    }, []);

    const submitContent = useCallback(async (chatId: number) => {
        try {
            contentRef.current.chatId = chatId;

            const messageId = await post<string>('/api/message/send', contentRef.current);

            contentRef.current.id = +messageId;

            dispatch(setMessages([...Object.values(messages).flat(),
            { content: contentRef.current.content, emote: contentRef.current.emote, id: contentRef.current.id }]));
        }
        finally {
            setContent((prevContent) => ({ ...prevContent, content: '' }));
        }
    }, [dispatch, messages]);

    const handleTextAreaChange = useCallback((e: ChangeEvent<HTMLTextAreaElement>) => {
        setContent({
            ...content,
            content: e.target.value
        });
        contentRef.current = { ...content, content: e.target.value };
    }, []);

    const handleEmote = useCallback(async (emote: boolean | null, messageId: number) => {
        emoteModel.emote = emote;
        emoteModel.messageId = messageId;

        await post<string>('/api/message/setEmote', emoteModel);

        dispatch(setMessages(Object.values(messages).flat().map((message) => (
            message.id === messageId ? { ...message, emote: emote } : message
        ))));
    }, [emoteModel, dispatch, messages]);

    return (
        <>
            <div className="chat-scrollbar">
                <List
                    className="list-display"
                    itemLayout="horizontal"
                    locale={{ emptyText: true }}
                    dataSource={Object.values(messages).flat()}
                    split={false}
                    renderItem={(item) => (
                        <List.Item>
                            <Card
                                className="chat-bubble"
                                onMouseEnter={(e) => {
                                    e.currentTarget.classList.add("show-reactions");
                                }}
                                onMouseLeave={(e) => {
                                    e.currentTarget.classList.remove("show-reactions");
                                }}
                            >
                                <Typography.Text>{item.content}</Typography.Text>
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