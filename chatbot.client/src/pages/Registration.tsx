import { useState, useCallback, ChangeEvent } from 'react';
import { RegistrationFormData } from '../models/RegistrationFormData';
import { Button, Form, Input, Typography } from 'antd';
import '../assets/styles/form.css';
import { post } from '../services/ApiClient';
import { useNavigate } from 'react-router-dom';

export const Registration = () => {
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [formData, setFormData] = useState<RegistrationFormData>({
    userName: '',
    email: '',
    password: '',
    confirmPassword: '',
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
    setIsSubmitting(true);
    try {
      await post('/api/account/registration', formData, { skipAuthHeader: true });

      navigate('/login');
    }
    finally {
      setIsSubmitting(false);
    }
  }, [formData, setIsSubmitting]);

  const validateConfirmPassword = (_: any, value: string) => {
    if (!value || formData.password === value) {
      return Promise.resolve();
    }
    return Promise.reject(new Error('Passwords do not match!'));
  };

  return (
    <>
      <Typography.Title className='text-align-center'>Registration form</Typography.Title>

      <Form
        className='form-center registration'
        labelCol={{ span: 8 }}
        onFinish={handleSubmit}
      >
        <Form.Item
          label='User Name'
          name='userName'
          rules={[
            { required: true, message: 'Please enter a user name' },
            { type: 'string', min: 3, message: 'User name must be at least 3 characters' },
          ]}
        >
          <Input
            type='userName'
            name='userName'
            value={formData.userName}
            onChange={handleChange}
          />
        </Form.Item>

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
          rules={[
            { required: true, message: 'Please enter a password' },
            { type: 'string', min: 6, message: 'Password must be at least 6 characters' },
          ]}
        >
          <Input.Password
            type='password'
            name='password'
            value={formData.password}
            onChange={handleChange}
          />
        </Form.Item>

        <Form.Item
          label='Confirm Password'
          name='confirmPassword'
          rules={[
            { required: true, message: 'Please enter a password' },
            { type: 'string', min: 6, message: 'Password must be at least 6 characters' },
            { validator: validateConfirmPassword, },
          ]}
        >
          <Input.Password
            type='confirmPassword'
            name='confirmPassword'
            value={formData.confirmPassword}
            onChange={handleChange}
          />
        </Form.Item>

        <Form.Item
          className='text-align-center'
          wrapperCol={{ offset: 8 }}
        >
          <Button
            type='primary'
            htmlType='submit'
            disabled={isSubmitting}>
            {isSubmitting ? 'Registering...' : 'Register'}
          </Button>
        </Form.Item>
      </Form>
    </>
  );
};