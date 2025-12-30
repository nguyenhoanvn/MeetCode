
import { Link } from "react-router-dom";
import useForgotPassword from "../hooks/useForgotPassword";
import { useState } from "react";

export default function ForgotPasswordPage() {
    const { forgotForm, otpForm, loading, error, errorField, sendOtpButton, verifyOtpButton,
        handleForgotFormChange, handleOtpFormChange, 
        handleForgotPasswordSubmit, handleVerifyOTPSubmit } = useForgotPassword();

    const inputClass = (fieldName: string) =>
    `border-2 px-4 py-2 w-1/1 rounded-2xl text-md duration-300 focus:outline-none
     ${
        errorField === fieldName
            ? "border-red-500 focus:border-red-500"
            : "focus:border-[#1e3a8a]"
     }`;

    const [randomNumber] = useState(() => Math.floor(Math.random() * 20));
    return(
        <div className="h-screen w-screen ">
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
                <div className="flex flex-col bg-[#161b22] px-10 py-3 justify-center">
                    <div className="flex flex-col gap-10">
                        <div className="">
                            <p className="text-2xl text-gray-200 font-black">Forgot Password?<br/>
                            Here we go again...</p>
                            <p className="text-sm font-extralight text-gray-200 mt-3">
                                You know the deal, just input the email and find something to eat when we do absolute everything for you
                            </p>
                        </div>
                        <div className="flex flex-col gap-10">
                            
                            <form onSubmit={(e) => {
                                e.preventDefault();
                                handleForgotPasswordSubmit();
                            }}>
                                <div className="flex flex-col gap-3">
                                    <div className="flex flex-col gap-5 relative">
                                        <input type="email"
                                        name="email"
                                        value={forgotForm.email}
                                        required
                                        autoFocus
                                        autoComplete="off"
                                        onChange={handleForgotFormChange}
                                        placeholder="Email..."
                                        className={inputClass("email")}/>

                                        <button type="submit" className={`${sendOtpButton.status ? "disabled text-gray-500" : "cursor-pointer hover:border-transparent hover:text-[#1e3a8a]"}
                                        absolute right-1/30 top-1/4
                                        bg-transparent 
                                        duration-500`}>
                                            {sendOtpButton.cooldown > 0 ? `Sent (${(sendOtpButton.cooldown)}s)` : "Send OTP"}
                                        </button>
                                    </div>
                                </div>
                            </form>
                            <form onSubmit={(e) => {
                                e.preventDefault();
                                handleVerifyOTPSubmit();
                            }}>
                                <div className="flex flex-col gap-3">
                                    <div className="flex flex-col gap-5">
                                        <input type="text"
                                        name="code"
                                        value={otpForm.code}
                                        required
                                        autoFocus
                                        maxLength={6}
                                        autoComplete="off"
                                        onChange={handleOtpFormChange}
                                        placeholder="363636"
                                        className={inputClass("code")}/>

                                        {error ? 
                                        (<div className="text-red-400 text-sm flex items-center gap-1">
                                            <span className="material-symbols-outlined text-sm!">
                                                error
                                            </span>
                                            {error}
                                        </div>) : (<></>)}

                                        <button type="submit" className={`${verifyOtpButton.status ? "bg-gray-700 cursor-not-allowed" : ""} 
                                        mt-5 bg-transparent border-2 border-[#1e3a8a] py-3 rounded-xl cursor-pointer
                                        hover:border-transparent hover:bg-[#1e3a8a]
                                        duration-500`}>
                                            {verifyOtpButton.status ? "Verifying..." : "Verify OTP"}
                                        </button>
                                    </div>
                                </div>
                            </form>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    )
}