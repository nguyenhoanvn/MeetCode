import { useState } from "react";
import { login, register } from "../api/authApi";

export function useAuth() {
    const [user, setUser] = useState(null);
    const [accessToken, setAccessToken] = useState(null);

    const handleLogin = async (email, password) => {
        const data = await login(email, password);
        setAccessToken(data.accessToken);
        setUser(data.user);
    };
    const handleRegister = async (email, password, displayName) => {
        const data = await register(email, password, displayName);
        setUser(data.user);
    };

    const logout = () => {
        setAccessToken(null);
        setUser(null);
    };

    return { user, accessToken, handleLogin, handleRegister, logout };
}