import { useState, useCallback, ChangeEvent } from 'react';
import { LoginFormData } from '../models/LoginFormData';
import { Button, Form, Input, Typography } from 'antd';

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

        const response = await fetch('/api/account/login', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(formData),
        });

        const result = await response.json();
        console.log('Login successful:', result);

        setIsSubmitting(false);
    }, [formData, setIsSubmitting]);

    return (
        <>
            <Typography.Title className='title-padding'>Login</Typography.Title>

            <Form
                className='form-width'
                labelCol={{ span: 4 }}
                wrapperCol={{ span: 14 }}
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

                <Form.Item wrapperCol={{ offset: 9 }}>
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