import { useState } from "react";
import { login, register } from "../api/auth";
import { useNavigate } from "react-router-dom";
import { ApiProblemDetail } from "../types/system/apiProblemDetail";

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
    const [errorField, setErrorField] = useState<string | null>(null);

    const navigate = useNavigate();

    const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        setError(null);
        setErrorField(null);
        const {name, value} = e.target;
        setRegisterForm((prev) => ({...prev, [name]: value}));
    };

    const handleSubmit = async () => {
        setLoading(true);
        setError(null);
        try {
            const regResp = await register(registerForm);

            const loginRequest: LoginRequest = {
                email: registerForm.email,
                password: registerForm.password,
            };

            await login(loginRequest);
            navigate("/", { replace: true });
        } catch (err: unknown) {
            const apiError = err as ApiProblemDetail;
            if (apiError.errors) {
                const entries = Object.entries(apiError.errors ?? {});
                if (entries.length > 0) {
                    const [field, messages] = entries[0];
                    setErrorField(field.toLowerCase());       
                    setError(messages[0]);     
                }
            } else {
                setError("Unknown error");
            }
        } finally {
            setLoading(false);
        }
    };

    return {registerForm, handleChange, handleSubmit, loading, error, errorField};
}