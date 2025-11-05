import { useState } from "react";
import { useRegister } from "../hooks/useAuth";
import { register } from "../api/auth";
import NavigationBar from "../components/NavigationBar";

export default function RegisterPage() {
    const { response, loading, error } = useRegister();
    const [displayName, setDisplayName] = useState("");
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");

    const isFormValid = displayName && email && password;

    const handleSubmit = async (e) => {
        e.preventDefault();

        const newUser = {displayName, email, password};
        const result = await register(newUser);

        if (result) {
            alert("Added successfully");
            setDisplayName("");
            setEmail("");
            setPassword("");
        }
    }

    return (
        <div className="h-screen w-screen bg-amber-300 overflow-hidden">
            <NavigationBar/>
            <div className="flex justify-center h-screen w-screen">
                <div className="flex flex-col gap-10 h-3/4 w-5/12 bg-[#161B22] border-green-500 border-2 rounded-md px-10 py-15">
                    <p className="text-gray-200 font-black text-2xl">Join <span className="text-[#1E3A8A] font-bold">MeetCode</span>
                    <br/><span className="text-sm font-light">be ready to excel</span></p>
                    <form className="" onSubmit={handleSubmit}>
                        <div className="flex flex-col gap-5">
                            <div className="flex flex-col gap-3">
                                <input type="text" value={displayName}
                                required
                                onChange={(e) => setDisplayName(e.target.value)}
                                placeholder="Display Name..."
                                className="border-2 px-4 py-2 w-1/1 rounded-2xl text-md"/>
                            </div>
                            <div>
                                <input type="email" value={email}
                                required
                                onChange={(e) => setEmail(e.target.value)}
                                placeholder="Email..."
                                className="border-2 px-4 py-2 w-1/1 rounded-2xl text-md"/>
                            </div>
                            <div>
                                <input type="password" value={password}
                                required
                                onChange={(e) => setPassword(e.target.value)}
                                placeholder="Password..."
                                className="border-2 px-4 py-2 w-1/1 rounded-2xl text-md"/>
                            </div>
                            <button className="bg-green-600 text-white">Register</button>
                        </div>
                    </form>
                </div>
            </div>
        </div>
    );
}