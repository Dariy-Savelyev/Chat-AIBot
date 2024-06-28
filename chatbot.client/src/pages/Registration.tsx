import { useState, useCallback, ChangeEvent, FormEvent } from 'react';
import { FormData } from '../models/FormData';
import { FormErrors } from '../models/FormErros';

export const Registration = () => {
    const [formData, setFormData] = useState<FormData>({
        userName: '',
        email: '',
        password: '',
        confirmPassword: '',
      });
    
      const [errors, setErrors] = useState<FormErrors>({});
      const [isSubmitting, setIsSubmitting] = useState(false);
      const [submitError, setSubmitError] = useState<string | null>(null);
      
      const handleChange = useCallback((e: ChangeEvent<HTMLInputElement>) => {
        const { name, value } = e.target;
        setFormData((prevData) => ({
          ...prevData,
          [name]: value,
        }));
      }, []);
    
      const validate = (): FormErrors => {
        let formErrors: FormErrors = {};
        if (!formData.userName) formErrors.userName = 'Username is required';
        if (!formData.email) {
          formErrors.email = 'Email is required';
        } else if (!/\S+@\S+\.\S+/.test(formData.email)) {
          formErrors.email = 'Email address is invalid';
        }
        if (!formData.password) formErrors.password = 'Password is required';
        if (formData.password !== formData.confirmPassword) {
          formErrors.confirmPassword = 'Passwords do not match';
        }
        return formErrors;
      };
    
      const handleSubmit = useCallback(async (e: FormEvent) => {
        e.preventDefault();
        const formErrors = validate();
        if (Object.keys(formErrors).length === 0) {
          setIsSubmitting(true);
          setSubmitError(null);
          try {
            const response = await fetch('/api/account/registration', {
              method: 'POST',
              headers: {
                'Content-Type': 'application/json',
              },
              body: JSON.stringify(formData),
            });
    
            if (!response.ok) {
              throw new Error('Network response was not ok');
            }
    
            const result = await response.json();
            console.log('Form submitted successfully:', result);
          } catch (error) {
            console.error('There was a problem with the submission:', error);
            setSubmitError('There was a problem submitting the form. Please try again.');
          } finally {
            setIsSubmitting(false);
          }
        } else {
          setErrors(formErrors);
        }
      }, [formData, validate, setIsSubmitting, setSubmitError, setErrors]);
    
      return (
        <>
          <h1>Registration form</h1>
    
          <form onSubmit={handleSubmit}>
            <div>
              <label htmlFor="userName">Username</label>
              <input
                type="text"
                name="userName"
                value={formData.userName}
                onChange={handleChange}
              />
              {errors.userName && <span>{errors.userName}</span>}
            </div>
    
            <div>
              <label htmlFor="email">Email</label>
              <input
                type="email"
                name="email"
                value={formData.email}
                onChange={handleChange}
              />
              {errors.email && <span>{errors.email}</span>}
            </div>
    
            <div>
              <label htmlFor="password">Password</label>
              <input
                type="password"
                name="password"
                value={formData.password}
                onChange={handleChange}
              />
              {errors.password && <span>{errors.password}</span>}
            </div>
    
            <div>
              <label htmlFor="confirmPassword">Confirm Password</label>
              <input
                type="password"
                name="confirmPassword"
                value={formData.confirmPassword}
                onChange={handleChange}
              />
              {errors.confirmPassword && <span>{errors.confirmPassword}</span>}
            </div>
    
            <button type="submit" disabled={isSubmitting}>
              {isSubmitting ? 'Registering...' : 'Register'}
            </button>
            {submitError && <div>{submitError}</div>}
          </form>
        </>
      );
};