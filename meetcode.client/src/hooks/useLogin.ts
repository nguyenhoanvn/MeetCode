import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { login } from "../api/auth";

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