import { useState, useCallback, ChangeEvent } from 'react';
import { LoginFormData } from '../models/LoginFormData';
import { Button, Form, Input, Typography } from 'antd';
import { AccesTokenService } from '../services/AccessTokenService';
import { get, post } from '../services/ApiClient';
import '../assets/styles/form.css';
import { useNavigate } from 'react-router-dom';
import { useDispatch } from 'react-redux';
import { setChats } from '../store/slices/ChatSlice';
import { Chat } from '../models/GetAllChatModel';

export const Login = () => {
    const [isSubmitting, setIsSubmitting] = useState(false);
    const [formData, setFormData] = useState<LoginFormData>({
        email: '',
        password: '',
    });

    const navigate = useNavigate();
    const dispatch = useDispatch();

    const handleChange = useCallback((e: ChangeEvent<HTMLInputElement>) => {
        const { name, value } = e.target;
        setFormData((prevData) => ({
            ...prevData,
            [name]: value,
        }));
    }, []);

    const handleSubmit = useCallback(async () => {
        setIsSubmitting(true);
        try {
            const response = await post<string>('/api/account/login', formData, { skipAuthHeader: true });

            const accessToken = response;
            AccesTokenService.saveAccessToken(accessToken);

            const chatResponse = await get<Chat[]>('/api/chat/getAllChats');
            dispatch(setChats(chatResponse));

            navigate('/');
            console.log('Login successful accessToken:', accessToken);
        }
        finally {
            setIsSubmitting(false);
        }

    }, [formData, setIsSubmitting]);

    return (
        <>
            <Typography.Title className='text-align-center'>Login</Typography.Title>

            <Form
                className='form-center login'
                labelCol={{ span: 5 }}
                onFinish={handleSubmit}
            >
                <Form.Item
                    label='Email'
                    name='email'
                    rules={[
                        { required: true, message: 'Please enter an email' },
                        { type: 'email', message: 'Please enter a valid email' },
                    ]}
                >
                    <Input
                        type='email'
                        name='email'
                        value={formData.email}
                        onChange={handleChange}
                    />
                </Form.Item>

                <Form.Item
                    label='Password'
                    name='password'
                    rules={[{ required: true, message: 'Please enter a password' }]}
                >
                    <Input.Password
                        type='password'
                        name='password'
                        value={formData.password}
                        onChange={handleChange}
                    />
                </Form.Item>

                <Form.Item wrapperCol={{ offset: 12 }}>
                    <Button
                        type='primary'
                        htmlType='submit'
                        disabled={isSubmitting}>
                        {isSubmitting ? 'Logging in...' : 'Login'}
                    </Button>
                </Form.Item>
            </Form>
        </>
    );
};