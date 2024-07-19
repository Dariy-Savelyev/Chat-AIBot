import { useState, useCallback, ChangeEvent } from 'react';
import { LoginFormData } from '../models/LoginFormData';
import { Button, Form, Input, Typography } from 'antd';
import apiClient from '../services/apiClient';
import '../assets/styles/form.css';
import { Tokens } from '../models/Tokens';

export const Login = () => {
    const [formData, setFormData] = useState<LoginFormData>({
        email: '',
        password: '',
    });

    const [isSubmitting, setIsSubmitting] = useState(false);

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
            const response = await apiClient.post<string>('/api/account/login', formData);

            const accessToken = response.data;
            localStorage.setItem("accessToken", response.data)

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
                className='form-view'
                labelCol={{ span: 10 }}
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

                <Form.Item
                    className='text-align-center'
                    wrapperCol={{ offset: 10 }}
                >
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