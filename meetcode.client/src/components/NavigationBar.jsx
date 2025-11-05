import { useState } from "react";
import { useNavigate } from "react-router-dom";

export default function NavigationBar() {
    const navigate = useNavigate();

    const handleProblem = () => {
        navigate("/problems")
    }
    const handleRegister = () => {
        navigate("/auth/register")
    }
    return (
        <div className="flex flex-row bg-[#0D1117] border-red-200 border-b-2 items-center">
            <div className="p-4 no-underline">
                <a href="/" ><p className="text-white font-black px-3 py-1 hover:bg-[#1E3A8A] hover:rounded-2xl">Home</p></a>
            </div>
            <div className="p-4">
                <a href="/auth/register" className="text-white font-black px-3 py-1 hover:bg-[#1E3A8A] hover:rounded-2xl" onClick={handleRegister}>Register</a>
            </div>
        </div>
    )
}