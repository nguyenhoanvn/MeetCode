import { Link } from "react-router-dom";
import { useLogin } from "../hooks/useLogin";
import { useState } from "react";

export default function LoginPage() {
    const {loginForm, handleChange, handleSubmit, loading, error} = useLogin();

    const [randomNumber] = useState(() => Math.floor(Math.random() * 20));

    return (
        <div className="w-screen h-screen">
            <div className="grid grid-cols-[60%_40%] h-screen w-screen">
                <div className="bg-red relative w-full h-full">
                    {randomNumber === 1 ? 
                    (<video
                        className="absolute inset-0 w-full h-full object-cover"
                        src="/videos/meme/coding1.mp4"
                        autoPlay
                        loop
                        muted
                        playsInline
                    />) :
                    (<video
                        className="absolute inset-0 w-full h-full object-cover"
                        src="/videos/normal/coding1.mp4"
                        autoPlay
                        loop
                        muted
                        playsInline
                    />)}
                    
                    <div className="absolute p-50 inset-0 bg-black/30 flex flex-col">
                        <Link to="/"><p className="text-4xl font-black text-blue-300">MeetCode</p></Link>
                        <div className="mt-auto flex flex-col gap-8">
                            <div className="flex flex-col gap-2">
                                <p className="text-2xl text-slate-300 font-medium">Welcome to</p>
                                <p className="text-3xl font-medium"><Link to="/"><span className="text-blue-300">MeetCode</span></Link> Coding Platform</p>
                            </div>
                            <div>
                                <p className="text-sm text-gray-400">If you feel familiar with any of front end design here, you are not wrong</p>
                                <p className="text-sm text-gray-400">This website is full of hidden <a href="https://en.wikipedia.org/wiki/Easter_egg_(media)"><span className="font-medium hover:text-blue-300 text-blue-500 transition duration-100">easter egg</span></a>, hope the luck guide you find them all</p>
                            </div>
                        </div>
                    </div>
                </div>
                <div className="flex flex-col bg-[#161b22] px-10 py-3 gap-10 justify-center">
                    <div className="">
                        <p className="text-2xl text-gray-200 font-black">Welcome back!<br/>
                        Login your account <span className="text-sm font-light">or <Link to="/auth/register"><span className="font-medium hover:text-blue-300 text-blue-500 transition duration-100">Register</span></Link> to have one for $0.00!</span></p>
                        <p className="text-sm font-extralight text-gray-200 mt-3">Ready to be tortured?</p>
                        {error ? (
                            <p className="text-sm font-extralight text-red-600 mt-3">
                                {error}
                            </p>
                        ) : (<></>)}
                        
                    </div>
                    <div>
                        <form onSubmit={(e) => {
                            e.preventDefault();
                            handleSubmit();
                        }}>
                            <div className="flex flex-col gap-5">
                                <input type="email"
                                name="email"
                                value={loginForm.email}
                                required
                                autoFocus
                                autoComplete="off"
                                onChange={handleChange}
                                placeholder="Email..."
                                className="border-2 px-4 py-2 w-1/1 rounded-2xl text-md
                                duration-300
                                focus:outline-none
                                focus:border-[#1e3a8a]"/>

                                <input type="password"
                                name="password"
                                value={loginForm.password}
                                required
                                autoComplete="off"
                                onChange={handleChange}
                                placeholder="Password..."
                                className="border-2 px-4 py-2 w-1/1 rounded-2xl text-md
                                duration-300
                                focus:outline-none
                                focus:border-[#1e3a8a]"/>

                                <div className="flex justify-end gap-10">
                                    <a className="text-sm hover:underline" href="/auth/forgot-password">Forgot password?</a>
                                </div>

                                <button type="submit" className={`${loading ? "bg-gray-700 cursor-not-allowed" : ""} 
                                bg-transparent border-2 border-[#1e3a8a] py-3 rounded-xl cursor-pointer
                                hover:border-transparent hover:bg-[#1e3a8a]
                                duration-500`}>
                                    {loading ? "Logging in..." : "Log in"}
                                </button>
                            </div>
                        </form>
                    </div>
                </div>
            </div>
        </div>
    )
}