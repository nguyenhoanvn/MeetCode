import { useState } from "react";
import { register } from "../api/auth";
import { login } from "../api/auth";
import { useNavigate } from "react-router-dom";

interface RegisterForm {
    displayName: string;
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
        } catch (err: any) {
            setError(err.message ?? "Unknown error");
            console.log(err.message);
        } finally {
            setLoading(false);
        }
    };

    return {registerForm, handleChange, handleSubmit, loading, error};
}

interface LoginForm {
    email: string;
    password: string;
}

interface LoginResponse {
    isSuccessfully: boolean;
    message: string;
}

export const useLogin = () => {
    const [loginForm, setLoginForm] = useState<LoginForm>({email: "", password: ""});
    const [resp, setResp] = useState<LoginResponse>({isSuccessfully: true, message: ""})
    const [loading, setLoading] = useState<boolean>(false);
    const [error, setError] = useState<string | null>(null);

    const navigate = useNavigate();

    const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const {name, value} = e.target;
        setLoginForm((prev) => ({...prev, [name]: value}));
    };

    const handleSubmit = async () => {
        try {
            setLoading(true); 
            const response = await login(loginForm);
            setResp(response);
            if (response.isSuccessfully) {
                navigate("/");
            } else {
                console.log(response.message);
            }
        } catch (err: any) {
            setError(err.message ?? "Unknown error");
        } finally {
            setLoading(false);
        }
    }

    return {loginForm, resp, handleChange, handleSubmit, loading, error};
}