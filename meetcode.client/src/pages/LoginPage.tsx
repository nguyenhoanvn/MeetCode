import { useLogin } from "../hooks/useAuth";

export default function LoginPage() {
    const {loginForm, resp, handleChange, handleSubmit, loading, error} = useLogin();

    return (
        <div className="w-screen h-screen bg-amber-300">
            <div className="grid grid-cols-[60%_40%] h-screen w-screen">
                <div className="bg-white">
                    
                </div>
                <div className="flex flex-col bg-[#161b22] px-10 py-3 gap-10">
                    <div className="">
                        <p className="text-2xl text-gray-200 font-black">Welcome back!<br/>
                        Login your account</p>
                        <p className="text-sm font-extralight text-gray-200 mt-3">Ready to be tortured?</p>
                        <p className={`${!resp.isSuccessfully && resp.message != "" ? "block" : "hidden"} text-sm font-extralight text-red-600 mt-3`}>
                            {resp.message}
                        </p>
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
                                    {loading ? "Logging in" : "Log in"}
                                </button>
                            </div>
                        </form>
                    </div>
                </div>
            </div>
        </div>
    )
}