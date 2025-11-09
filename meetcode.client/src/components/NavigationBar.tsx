import logo from "../assets/images/logo.png";
import useNavBar from "../hooks/useNavBar";

export default function NavigationBar() {
    const {handleProblem, handleRegister, handleLogin, user, error, loading} = useNavBar();

    return (
        <div className="flex flex-row items-center gap-10 bg-[#0D1117] border-b px-0.5 py-0.75 shadow-[0_0_20px_2px_rgba(255,255,255,0.6)]">
            <a href="/" className="p-3 ml-5">
                <img className="w-7 h-7 duration-500 hover:scale-110" src={logo}></img>
            </a>
            {!loading && user.userId === null && (
                <>
                    <a href="/auth/register">
                        <p className="text-gray-200 font-black px-10 py-1 duration-500 hover:bg-[#1E3A8A] rounded-2xl" onClick={handleRegister}>Register</p>
                    </a>
                    <a href="/auth/login">
                        <p className="text-gray-200 font-black px-10 py-1 duration-500 hover:bg-[#1E3A8A] rounded-2xl" onClick={handleLogin}>Login</p>
                    </a>
                    <a href="/problems">
                        <p className="text-gray-200 font-black px-10 py-1 duration-500 hover:bg-[#1E3A8A] rounded-2xl" onClick={handleProblem}>Problems</p>
                    </a>
                </>
            )}
            {!loading && user.userId !== null && (
                <p className="text-lg">Hello {user.displayName}</p>
            )}
        </div>
    )
}