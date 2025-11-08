import { useState } from "react";
import { login, register } from "../api/auth";

interface RegisterForm {
    displayName: string;
    email: string;
    password: string;
}

interface LoginRequest {
    email: string;
    password: string;
}

export const useRegister = () => {
    const [registerForm, setRegisterForm] = useState<RegisterForm>({displayName: "", email: "", password: ""});
    const [loading, setLoading] = useState<boolean>(false);
    const [error, setError] = useState<string | null>(null);

    const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const {name, value} = e.target;
        setRegisterForm((prev) => ({...prev, [name]: value}));
    };

    const handleSubmit = async () => {
        setLoading(true);
        setError(null);
        try {
            await register(registerForm);
            const loginRequest: LoginRequest = {
                email: registerForm.email,
                password: registerForm.password,
            };
            await login(loginRequest);
        } catch (err: any) {
            setError(err.message ?? "Unknown error");
            console.log(err.message);
        } finally {
            setLoading(false);
        }
    };

    return {registerForm, handleChange, handleSubmit, loading, error};
}