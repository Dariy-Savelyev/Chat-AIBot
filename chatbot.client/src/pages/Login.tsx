import { useState, useCallback, ChangeEvent } from 'react';
import { LoginFormData } from '../models/LoginFormData';
import { Alert, Button, Form, Input, Typography } from 'antd';
import { AccesTokenService } from '../services/AccessTokenService';
import { post } from '../services/ApiClient';
import '../assets/styles/form.css';
import { useNavigate } from 'react-router-dom';

export const Login = () => {
    const [errorMessage, setErrorMessage] = useState<string | null>(null);
    const [isSubmitting, setIsSubmitting] = useState(false);
    const [formData, setFormData] = useState<LoginFormData>({
        email: '',
        password: '',
    });

    const navigate = useNavigate();

    const handleChange = useCallback((e: ChangeEvent<HTMLInputElement>) => {
        const { name, value } = e.target;
        setFormData((prevData) => ({
            ...prevData,
            [name]: value,
        }));
    }, []);
    
    const handleSubmit = useCallback(async () => {
        setErrorMessage(null);
        setIsSubmitting(true);
        try {
            const response = await post<string>('/api/account/login', formData, { skipAuthHeader: true });

            const accessToken = response;
            AccesTokenService.saveAccessToken(accessToken);

            navigate('/');
        }
        catch (error: any) {
            if (error.response) {
                if (error.response.status === 422) {
                    setErrorMessage('Invalid email or password');
                } else if (error.response.status === 403) {
                    setErrorMessage('Please try again later');
                } else {
                    setErrorMessage('An error occurred. Please try again.');
                }
            }
        }
        finally {
            setIsSubmitting(false);
        }
    }, [formData, setIsSubmitting]);

    return (
        <>
            <Typography.Title className='text-align-center'>Login</Typography.Title>

            <div className="alert-container">
                {errorMessage && (
                    <Alert
                        className='alert'
                        message={errorMessage}
                        type="error"
                        showIcon
                    />
                )}
            </div>

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
                        { type: 'email', message: 'Please enter a valid email' }
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