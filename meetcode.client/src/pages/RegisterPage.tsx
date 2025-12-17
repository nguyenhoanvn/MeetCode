import { useRegister } from "../hooks/useRegister";
import NavigationBar from "../components/NavigationBar";

export default function RegisterPage() {
    const {registerForm, handleChange, handleSubmit, loading, error, errorField} = useRegister();

    const inputClass = (fieldName: string) =>
    `border-2 px-4 py-2 w-1/1 rounded-2xl text-md duration-300 focus:outline-none
     ${
        errorField === fieldName
            ? "border-red-500 focus:border-red-500"
            : "focus:border-[#1e3a8a]"
     }`;


    return (
        <div className="h-screen w-screen overflow-hidden">
            <NavigationBar/>
            <div className="flex justify-center p-10">
                <div className="flex flex-col gap-10 min-h-fit min-w-fit bg-[#161B22] border-white border-2 rounded-md px-10 pt-15 pb-10">
                    <p className="text-gray-200 font-black text-2xl">Join <span className="text-[#1E3A8A] font-bold">MeetCode</span>
                    <br/><span className="text-sm font-light">be ready to excel</span></p>
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
    );
}