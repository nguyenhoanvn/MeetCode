import { useState } from "react";
import { useNavigate } from "react-router-dom";
import logo from '../assets/images/logo.png';

export default function NavigationBar() {
    const navigate = useNavigate();

    const handleProblem = () => {
        navigate("/problems")
    }
    const handleRegister = () => {
        navigate("/auth/register")
    }
    const handleLogin = () => {
        navigate("/auth/login")
    }
    return (
        <div class="flex flex-row items-center gap-10 bg-[#0D1117] border-b px-0.5 py-0.75 shadow-[0_0_20px_2px_rgba(255,255,255,0.6)]">
            <a href="/" className="p-3 ml-5">
                <img className="w-7 h-7 duration-500 hover:scale-110" src={logo}></img>
            </a>
            <a href="/auth/register">
                <p className="text-gray-200 font-black px-10 py-1 duration-500 hover:bg-[#1E3A8A] rounded-2xl" onClick={handleRegister}>Register</p>
            </a>
            <a href="/auth/login">
                <p className="text-gray-200 font-black px-10 py-1 duration-500 hover:bg-[#1E3A8A] rounded-2xl" onClick={handleLogin}>Login</p>
            </a>
        </div>
    )
}