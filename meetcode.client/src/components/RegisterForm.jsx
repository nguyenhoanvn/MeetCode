import { useState } from "react";

export default function RegisterForm({ onRegister }) {
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [displayName, setDisplayName] = useState("");
    const [errors, setErrors] = useState({});

    const handleSubmit = async (e) => {
        e.preventDefault();
        setErrors({}); // reset previous errors

        try {
            const data = await onRegister(email, password, displayName);
        } catch (err) {
            if (err.response?.status === 400) {
                const errorDetail = err.response.data;

                if (errorDetail.errors) {
                    setErrors(errorDetail.errors);
                } else {
                    setErrors({ general: ["Something went wrong."] });
                }
            } else {
                setErrors({ general: ["Network error, please try again."] });
            }
        }
    };

    return (
        <form
        onSubmit={handleSubmit}
        className="p-4 border rounded shadow-md max-w-sm ms-auto"
        >
        <h2 className="text-xl mb-4">Register</h2>

        {/* Display Name */}
        <input
            type="text"
            placeholder="Display Name"
            value={displayName}
            onChange={(e) => setDisplayName(e.target.value)}
            className="w-full p-2 border mb-1 rounded"
        />
        {errors.DisplayName &&
            <p className="text-red-500 text-sm">
                {errors.DisplayName[0]}
            </p>}

        {/* Email */}
        <input
            type="email"
            placeholder="Email"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
            className="w-full p-2 border mb-1 rounded"
        />
        {errors.Email &&
            <p className="text-red-500 text-sm">
                { errors.Email[0]}
            </p>}

        {/* Password */}
        <input
            type="password"
            placeholder="Password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            className="w-full p-2 border mb-1 rounded"
        />
        {errors.Password &&
            <p className="text-red-500 text-sm">
                {errors.Password[0]}
            </p>}

        <button
            type="submit"
            className="w-full bg-green-600 text-white p-2 rounded mt-2"
        >
            Register
        </button>

        {/* General error */}
        {errors.general &&
            errors.general.map((msg, i) => (
            <p key={i} className="text-red-500 mt-2">
                {msg}
            </p>
            ))}
        </form>
    );
}
