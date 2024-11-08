import { List } from "antd";
import { MessageItem } from './MessageItem';
import { MessageListProps } from '../../models/PropsModels/MessageListProps';
import React from "react";

export const MessageList = React.memo(({ messages, userId, isInChat, handleEmote }: MessageListProps) => (
    <div className="chat-scrollbar">
        <List
            itemLayout="horizontal"
            locale={{ emptyText: true }}
            dataSource={Object.values(messages).flat()}
            split={false}
            renderItem={(item) => (
                <MessageItem
                    item={item}
                    userId={userId}
                    isInChat={isInChat}
                    handleEmote={handleEmote}
                />
            )}
        />
    </div>
));