import { useState, useCallback, ChangeEvent } from 'react';
import { LoginFormData } from '../models/LoginFormData';
import { Button, Form, Input, Typography } from 'antd';
import apiClient from '../services/apiClient';
import '../assets/styles/form.css';

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
            const response = await apiClient.post('/api/account/login', formData);

            const result = response.data;
            console.log('Login successful:', result);
        }
        finally {
            setIsSubmitting(false);
        }

    }, [formData, setIsSubmitting]);

    return (
        <>
            <Typography.Title className='text-align-center'>Login</Typography.Title>

            <Form
                className='form-wrapper'
                labelCol={{ span: 13 }}
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
                        className='input-login-width'
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
                        className='input-login-width'
                        type='password'
                        name='password'
                        value={formData.password}
                        onChange={handleChange}
                    />
                </Form.Item>

                <Form.Item wrapperCol={{ offset: 22 }}>
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