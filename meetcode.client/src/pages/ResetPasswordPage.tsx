import { Link } from "react-router-dom";
import useResetPassword from "../hooks/useResetPassword";

export default function ResetPasswordPage() {
    const {resetPasswordForm, loading, error, handleResetPasswordFormChange, handleResetPasswordSubmit} = useResetPassword();

    return(
        <div className="h-screen w-screen">
            <div className="grid grid-cols-[60%_40%] h-screen w-screen">
                <div className="bg-red relative w-full h-full">
                    {1 != 1 ? 
                    (<video
                        className="absolute inset-0 w-full h-full object-cover"
                        src="/videos/meme/skeletonDance.mp4"
                        autoPlay
                        loop
                        muted
                        playsInline
                    />) :
                    (<video
                        className="absolute inset-0 w-full h-full object-cover"
                        src="/videos/normal/coding.mp4"
                        autoPlay
                        loop
                        muted
                        playsInline
                    />)}
                    
                    <div className="absolute p-50 inset-0 bg-black/60 flex flex-col">
                        <Link to="/"><p className="text-4xl font-black text-blue-300">MeetCode</p></Link>
                        <div className="mt-auto flex flex-col gap-8">
                            <div className="flex flex-col gap-2">
                                <p className="text-2xl text-slate-300 font-medium">Welcome to</p>
                                <p className="text-3xl font-medium"><span className="text-blue-300">MeetCode</span> Coding Platform</p>
                            </div>
                            <div>
                                <p className="text-sm text-gray-400">If you feel familiar with any of front end design here, you are not wrong</p>
                                <p className="text-sm text-gray-400">This website is full of hidden <a href="https://en.wikipedia.org/wiki/Easter_egg_(media)"><span className="font-medium hover:text-blue-300 text-blue-500 transition duration-100">easter egg</span></a>, hope the luck guide you find them all</p>
                            </div>
                        </div>
                    </div>
                </div>
                <div className="flex flex-col bg-[#161b22] px-10 py-3 justify-center gap-15">
                    <div className="">
                        <p className="text-2xl text-gray-200 font-black">
                            Reset Password
                        </p>
                        <p className="text-sm font-extralight text-gray-200 mt-3">
                            Enter the new password and confirm it, make sure it's different from your old password and please don't forget it again
                        </p>
                    </div>
                    <div className="">
                        <form onSubmit={(e) => {
                            e.preventDefault();
                            handleResetPasswordSubmit();
                        }}>
                            <div className="flex flex-col gap-1">
                                <div className="flex flex-col gap-8">
                                    <input type="password"
                                    name="newPassword"
                                    value={resetPasswordForm.newPassword}
                                    required
                                    autoFocus
                                    autoComplete="off"
                                    onChange={handleResetPasswordFormChange}
                                    placeholder="New password..."
                                    className={`${error ? "border-red-600" : "focus:border-[#1e3a8a]"} border-2 px-4 py-2 w-1/1 rounded-2xl text-md
                                    duration-300
                                    focus:outline-none
                                    `}/>


                                    <div className="flex flex-col gap-2">
                                        <input type="password"
                                        name="confirmPassword"
                                        value={resetPasswordForm.confirmPassword}
                                        required
                                        autoFocus
                                        autoComplete="off"
                                        onChange={handleResetPasswordFormChange}
                                        placeholder="Confirm password..."
                                        className={`${error ? "border-red-600" : "focus:border-[#1e3a8a]"} border-2 px-4 py-2 w-1/1 rounded-2xl text-md
                                        duration-300
                                        focus:outline-none
                                        `}/>
                                    </div>
                                </div>
                                
                                {error ? 
                                        (<div className="text-red-400 text-sm flex items-center gap-1">
                                            <span className="material-symbols-outlined text-sm!">
                                                error
                                            </span>
                                            {error}
                                        </div>) : (<></>)}

                                <button type="submit" className={`${loading ? "bg-gray-700 cursor-not-allowed" : ""} 
                                bg-transparent border-2 border-[#1e3a8a] py-3 rounded-xl cursor-pointer mt-10
                                hover:border-transparent hover:bg-[#1e3a8a]
                                duration-500`}>
                                    {loading ? "Loading..." : "Change password"}
                                </button>
                            </div>
                        </form>
                    </div>
                </div>
            </div>
        </div>
    )
}