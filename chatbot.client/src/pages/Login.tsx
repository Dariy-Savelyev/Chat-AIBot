import { useState, useCallback, ChangeEvent, FormEvent } from 'react';
import { LoginFormData } from '../models/LoginFormData';
import { FormErrors } from '../models/FormErros';

export const Login = () => {
    const [formData, setFormData] = useState<LoginFormData>({
        email: '',
        password: '',
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
        const formErrors: FormErrors = {};
        if (!formData.email) {
            formErrors.email = 'Email is required';
        } else if (!/\S+@\S+\.\S+/.test(formData.email)) {
            formErrors.email = 'Email address is invalid';
        }
        if (!formData.password) {
            formErrors.password = 'Password is required';
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
                const response = await fetch('/api/account/login', {
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
                console.log('Login successful:', result);
            } catch (error) {
                console.error('There was a problem with the login:', error);
                setSubmitError('There was a problem logging in. Please try again.');
            } finally {
                setIsSubmitting(false);
            }
        } else {
            setErrors(formErrors);
        }
    }, [formData, validate, setIsSubmitting, setSubmitError, setErrors]);

    return (
        <>
            <h1>Login</h1>
            <form onSubmit={handleSubmit}>
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

                <button type="submit" disabled={isSubmitting}>
                    {isSubmitting ? 'Logging in...' : 'Login'}
                </button>
                {submitError && <div>{submitError}</div>}
            </form>
        </>
    );
};