import { useState, useEffect } from "react";
import { profileMinimal } from "../api/user/profile";
import { useNavigate } from "react-router-dom";
import { logout } from "../api/user/auth";
import { ApiProblemDetail } from "../types/system/apiProblemDetail";

interface User {
    userId: string | null;
    displayName: string;
}

export default function useNavBar() {
    const [user, setUser] = useState<User>({ userId: null, displayName: "Guest" });
    const [error, setError] = useState<string | null>(null);
    const [loading, setLoading] = useState(false);

    const navigate = useNavigate();

    const handleProblem = () => navigate("/problems")

    const handleLogin = () => navigate("/auth/login")

    const handleLogout = async () => {
        await logout();
        localStorage.removeItem("accessToken");
        navigate("/");
    }

    const handleStatus = async () => {
        try {
            setLoading(true);
            const response: User = await profileMinimal();
            setUser(response);
        } catch (err: any) {
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

    useEffect(() => {
        handleStatus();
    }, []);

    return { handleProblem, handleLogout, handleLogin, user, error, loading };
}