import { Button, Card, Input, List, Space, Typography } from "antd";
import { ChangeEvent, useCallback, useEffect, useRef, useState } from "react";
import { MessageModel } from "../models/MessageModel";
import { get, post } from "../services/ApiClient";
import { useParams } from "react-router-dom";
import '../assets/styles/chat.css';
import { GetAllMessageModel } from "../models/GetAllMessageModel";
import { useDispatch, useSelector } from "react-redux";
import { setMessages } from "../store/slices/MessageSlice";
import { MessageStateModel } from "../store/models/MessageStateModel";

export const SelectedChat = () => {
    const [content, setContent] = useState<MessageModel>({
        content: '',
        chatId: 0,
    });

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

            await post<string>('/api/message/send', contentRef.current);

            dispatch(setMessages([...Object.values(messages).flat(), { content: contentRef.current.content }]));
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
                            <Card className="chat-bubble">
                                <Typography.Text>{item.content}</Typography.Text>
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