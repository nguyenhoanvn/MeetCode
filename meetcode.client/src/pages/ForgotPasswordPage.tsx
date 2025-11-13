
import useForgotPassword from "../hooks/useForgotPassword";

export default function ForgotPasswordPage() {
    const { forgotForm, otpForm, loading, error, sendOtpButton, verifyOtpButton,
        handleForgotFormChange, handleOtpFormChange, 
        handleForgotPasswordSubmit, handleVerifyOTPSubmit } = useForgotPassword();

    return(
        <div className="h-screen w-screen bg-amber-300">
            <div className="grid grid-cols-[60%_40%] h-screen w-screen">
                <div className="bg-white">

                </div>
                <div className="flex flex-col bg-[#161b22] px-10 py-3 justify-center">
                    <div className="">
                        <div className="">
                            <p className="text-2xl text-gray-200 font-black">Forgot Password?<br/>
                            Here we go again...</p>
                            <p className="text-sm font-extralight text-gray-200 mt-3">
                                You know the deal, just input the email and find something to eat when we do absolute everything for you
                            </p>
                        </div>
                        <div className="flex flex-col gap-1">
                            
                            <form onSubmit={(e) => {
                                e.preventDefault();
                                handleForgotPasswordSubmit();
                            }}>
                                <div className="flex flex-col gap-3">
                                    <p className={`h-5 w-full text-md font-extralight text-red-600 mt-3`}>
                                        {sendOtpButton.message}
                                    </p>
                                    <div className="flex flex-col gap-5 relative">
                                        <input type="email"
                                        name="email"
                                        value={forgotForm.email}
                                        required
                                        autoFocus
                                        autoComplete="off"
                                        onChange={handleForgotFormChange}
                                        placeholder="Email..."
                                        className={`border-2 px-4 py-2 w-1/1 rounded-2xl text-md
                                        ${sendOtpButton.message != "" ? "border-red-600" : "focus:border-[#1e3a8a]"} 
                                        duration-300
                                        focus:outline-none`}/>

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
                                    <p className={`h-5 w-full text-md font-extralight text-red-600 mt-3`}>
                                        {verifyOtpButton.message}
                                    </p>
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
                                        className={`border-2 px-4 py-2 w-1/1 rounded-2xl text-md
                                        ${verifyOtpButton.message != "" ? "border-red-600" : "focus:border-[#1e3a8a]"}
                                        duration-300
                                        focus:outline-none`}/>

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