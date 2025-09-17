import { useState } from "react";
import { useNavigate } from "react-router-dom";

export default function LoginForm({ onLogin }) {
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [error, setError] = useState({});
    const navigate = useNavigate();

    const handleSubmit = async (e) => {
        e.preventDefault();
        setError({});
        try {
            const data = await onLogin(email, password);
            navigate("/");
        } catch (err) {
            if (err.response?.status === 400) {
                const errorDetail = err.response.data;
                if (errorDetail.errors) {
                    setError(errorDetail.errors);
                } else {
                    setError({general: ["Something went wrong."]});
                }
            } else {
                setError({general: ["Network error. Please try again"]});
            }
        }        
    };

    return (
        <form onSubmit={handleSubmit} className="p-4 border rounded shadow-md max-w-sm mx-auto">
            <h2 className="text-xl mb-4">Login</h2>
            <input type="email"
                placeholder="Email"
                value={email}
                onChange={(e) => setEmail(e.target.value)}
                className="w-full p-2 border mb-2 rounded"
                />
            {error.Email && <p>{error.Email[0]}</p>}
            <input type="password"
                placeholder="Password"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
                className="w-full p-2 border mb-2 rounded"
                />
            {error.Password && <p>{error.Password[0]}</p>}
            <button type="submit" className="w-full bg-blue-600 text-white p-2 rounded">
                Login
            </button>
            {error.general && <p style={{color: "red"}}>{error.general[0]}</p>}
        </form>
    )
}