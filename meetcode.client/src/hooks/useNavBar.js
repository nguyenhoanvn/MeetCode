import { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { profileMinimal } from "../api/profile";

export default function useNavBar() {
    const [user, setUser] = useState({userId: null, displayName: "Guest"});
    const [error, setError] = useState(null);
    const [loading, setLoading] = useState(false);

    const handleProblem = () => {
        navigate("/problems")
    }

    const handleRegister = () => {
        navigate("/auth/register")
    }

    const handleLogin = () => {
        navigate("/auth/login")
    }
    
    const handleStatus = async () => {
        try {
            setLoading(true);
            const response = await profileMinimal();
            setUser(response);
            console.log(response);
        } catch (err) {
            setError(err.message);
        } finally {
            setLoading(false);
        }
    }

    useEffect(() => {
        handleStatus();
    }, []);

    return {handleProblem, handleRegister, handleLogin, user, error, loading};
}