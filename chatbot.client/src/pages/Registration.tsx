import { useState, useCallback, ChangeEvent } from 'react';
import { RegistrationFormData } from '../models/RegistrationFormData';
import { Button, Form, Input, Typography } from 'antd';

export const Registration = () => {
  const [formData, setFormData] = useState<RegistrationFormData>({
    userName: '',
    email: '',
    password: '',
    confirmPassword: '',
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

    const response = await fetch('/api/account/registration', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(formData),
    });

    if (response.status == 200) {
      console.log('Form submitted successfully');
    } else {
       console.log(response.text());
    }

    setIsSubmitting(false);
  }, [formData, setIsSubmitting]);

  return (
    <>
      <Typography.Title style={{ paddingLeft: 160 }}>Registration form</Typography.Title>

      <Form
        labelCol={{ span: 8 }}
        wrapperCol={{ span: 14 }}
        style={{ maxWidth: 500 }}
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
            { type: 'string', min: 6, message: 'User name must be at least 3 charachters' },
          ]}
        >
          <Input.Password
            type='confirmPassword'
            name='confirmPassword'
            value={formData.confirmPassword}
            onChange={handleChange}
          />
        </Form.Item>

        <Form.Item wrapperCol={{ offset: 13 }}>
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