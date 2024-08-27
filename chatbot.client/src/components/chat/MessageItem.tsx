import { Button, Card, ConfigProvider, List, Typography } from "antd";
import likeIcon from '../../assets/images/likeIcon.png';
import dislikeIcon from '../../assets/images/dislikeIcon.png';
import { MessageItemProps } from '../../models/PropsModels/MessageItemProps';
import React from "react";

export const MessageItem = React.memo(({ item, userId, isInChat, handleEmote }: MessageItemProps) => (
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
));