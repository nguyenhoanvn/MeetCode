import { useState } from "react";
import { useRegister } from "../hooks/useAuth";
import { register } from "../api/auth";

export default function RegisterPage() {
    const { response, loading, error } = useRegister();
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");

    const handleSubmit = async (e) => {
        e.preventDefault();
        if (!email || !password) return alert("please fill all");

        const newUser = {email, password};
        const result = await register(newUser);

        if (result) {
            alert("Added successfully");
            setEmail("");
            setPassword("");
        }
    }

    return (
        <div className="h-screen w-screen bg-amber-300">
            <div className="h-1/2 w-1/2 bg-red-300">
                <form onSubmit={handleSubmit}>
                    <div>
                        <label>Email</label>
                        <input type="email" value={email}
                        onChange={(e) => setEmail(e.target.value)}
                        placeholder="enter email"/>
                    </div>
                    <div>
                        <label>Password</label>
                        <input type="password" value={password}
                        onChange={(e) => setPassword(e.target.value)}
                        placeholder="enter password"/>
                    </div>
                    <button type="submit" disabled={loading}>
                        {loading ? "Adding.." : "Register"}
                    </button>
                </form>
            </div>
        </div>
    );
}