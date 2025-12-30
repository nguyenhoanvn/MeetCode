import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { login } from "../api/auth";
import { ApiProblemDetail } from "../types/system/apiProblemDetail";

interface LoginForm {
    email: string;
    password: string;
}


export const useLogin = () => {
    const [loginForm, setLoginForm] = useState<LoginForm>({email: "", password: ""});
    const [loading, setLoading] = useState<boolean>(false);
    const [error, setError] = useState<string | null>(null);

    const navigate = useNavigate();

    const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        setError(null);
        const {name, value} = e.target;
        setLoginForm((prev) => ({...prev, [name]: value}));
    };

    const handleSubmit = async () => {
        try {
            setLoading(true); 
            const response = await login(loginForm);

            localStorage.setItem("accessToken", response.accessToken);

            navigate("/", { replace: true });
        } catch (err: unknown) {
            const apiError = err as ApiProblemDetail;
            if (apiError.errors) {
                const entries = Object.entries(apiError.errors ?? {});
                if (entries.length > 0) {
                    const [field, messages] = entries[0];      
                    setError(messages[0]); 
                }
            } else {
                setError("Unknown error");
            }    
        } finally {
            setLoading(false);
        }
    }

    return {loginForm, handleChange, handleSubmit, loading, error};
}