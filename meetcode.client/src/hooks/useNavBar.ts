    import { useState, useEffect } from "react";
    import { profileMinimal } from "../api/profile";
    import { useNavigate } from "react-router-dom";

    interface User {
        userId: string | null;
        displayName: string;
    }

    export default function useNavBar() {
        const [user, setUser] = useState<User>({userId: null, displayName: "Guest"});
        const [error, setError] = useState<string | null>(null);
        const [loading, setLoading] = useState(false);

        const navigate = useNavigate();

        const handleProblem = () => navigate("/problems")

        const handleRegister = () => navigate("/auth/register")

        const handleLogin = () => navigate("/auth/login")
        
        const handleStatus = async () => {
            try {
                setLoading(true);
                const response:User = await profileMinimal();
                setUser(response);
                console.log(response);
            } catch (err: any) {
                setError(err.message ?? "Unknown error");
            } finally {
                setLoading(false);
            }
        }

        useEffect(() => {
            handleStatus();
        }, []);

        return {handleProblem, handleRegister, handleLogin, user, error, loading};
    }