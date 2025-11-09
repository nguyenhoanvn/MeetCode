import useResetPassword from "../hooks/useResetPassword"

export default function ForgotPasswordPage() {
    const {forgotForm, loading, error, message, 
        handleForgotPasswordChange, handleForgotPasswordSubmit, 
        handleResetPasswordChange, handleResetPasswordSubmit} = useResetPassword();

    return(
        <div className="h-screen w-screen bg-amber-300">
            <div className="grid grid-cols-[60%_40%] h-screen w-screen">
                <div className="bg-white">

                </div>
                <div className="flex flex-col bg-[#161b22] px-10 py-3 gap-10">
                    <div className="">
                        <p className="text-2xl text-gray-200 font-black">Forgot Password?<br/>
                        Here we go again...</p>
                        <p className="text-sm font-extralight text-gray-200 mt-3">
                            You know the deal, just input the email and find something to eat when we do absolute everything for you
                        </p>
                        <p className={`${message != "" ? "block" : "hidden"} text-sm font-extralight text-red-600 mt-3`}>
                            {message}
                        </p>
                    </div>
                    <div>
                        <form onSubmit={(e) => {
                            e.preventDefault();
                            handleForgotPasswordSubmit();
                        }}>
                            <div className="flex flex-col gap-5">
                                <input type="email"
                                name="email"
                                value={forgotForm.email}
                                required
                                autoFocus
                                autoComplete="off"
                                onChange={handleForgotPasswordChange}
                                placeholder="Email..."
                                className="border-2 px-4 py-2 w-1/1 rounded-2xl text-md
                                duration-300
                                focus:outline-none
                                focus:border-[#1e3a8a]"/>

                                <button type="submit" className={`${loading ? "bg-gray-700 cursor-not-allowed" : ""} 
                                bg-transparent border-2 border-[#1e3a8a] py-3 rounded-xl cursor-pointer
                                hover:border-transparent hover:bg-[#1e3a8a]
                                duration-500`}>
                                    {loading ? "Loading..." : "Send OTP"}
                                </button>
                            </div>
                        </form>
                    </div>
                </div>
            </div>
        </div>
    )
}