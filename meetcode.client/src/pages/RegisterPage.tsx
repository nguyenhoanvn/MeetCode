import { useRegister } from "../hooks/useRegister";
import NavigationBar from "../components/NavigationBar";
import logo from "../assets/images/logo.png";
import { Link } from "react-router-dom";

export default function RegisterPage() {
    const {registerForm, handleChange, handleSubmit, loading, error, errorField} = useRegister();

    const inputClass = (fieldName: string) =>
    `border-2 px-4 py-2 w-1/1 rounded-2xl text-md duration-300 focus:outline-none
     ${
        errorField === fieldName
            ? "border-red-500 focus:border-red-500"
            : "focus:border-[#1e3a8a]"
     }`;

     const randomNumber = Math.floor(Math.random() * 20);


    return (
        <div className="w-screen h-screen bg-amber-300">
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
                <div className="flex flex-col bg-[#161b22] px-10 py-3 gap-10 justify-center">
                    <div className="">
                        <p className="text-gray-200 font-black text-2xl">Join <span className="text-[#1E3A8A] font-bold">MeetCode</span>
                        <br/><span className="text-sm font-light">be ready to excel</span></p>
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
                                <div className="flex flex-col gap-3">
                                    <input type="text" 
                                    name="displayName"
                                    value={registerForm.displayName}
                                    required
                                    autoComplete="off"
                                    autoFocus
                                    onChange={handleChange}
                                    placeholder="Display Name..."
                                    className={inputClass("displayName")}/>
                                </div>
                                <div>
                                    <input type="email" 
                                    name="email"
                                    value={registerForm.email}
                                    required
                                    autoComplete="off"
                                    onChange={handleChange}
                                    placeholder="Email..."
                                    className={inputClass("email")}/>
                                </div>
                                <div>
                                    <input type="password" 
                                    name="password"
                                    value={registerForm.password}
                                    required
                                    autoComplete="off"
                                    onChange={handleChange}
                                    placeholder="Password..."
                                    className={inputClass("password")}/>
                                </div>
                                {error ? 
                                (<div className="text-red-400 text-sm flex items-center gap-1">
                                    <span className="material-symbols-outlined text-sm!">
                                        error
                                    </span>
                                    {error}
                                </div>) : (<></>)}
                                <div className="mt-4">
                                    <p className="text-sm font-light">By clicking Register, you really agree with MeetCode's terms of service and privacy policy?</p>
                                    <p className="text-sm font-light">or <Link to="/auth/login"><span className="font-medium hover:text-blue-300 text-blue-500 transition duration-100">Login</span></Link> instead</p>
                                </div>
                                <button type="submit" disabled={loading || !!error} className={`${loading || error ? "bg-gray-700 disabled" : "cursor-pointer hover:border-transparent hover:bg-[#1e3a8a]"} 
                                border-2 border-[#1e3a8a] py-3 rounded-xl
                                duration-500`}>
                                    {loading ? "Registering..." : "Register"}
                                </button>
                            </div>
                        </form>
                    </div>
                </div>
            </div>
        </div>
    );
}