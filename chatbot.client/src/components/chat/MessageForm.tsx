import { Button, Input, Space } from "antd";
import { MessageFormProps } from '../../models/PropsModels/MessageFormProps';

export const MessageForm = ({ isInChat, content, handleTextAreaChange, submitContent, chatId }: MessageFormProps) => (
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
);