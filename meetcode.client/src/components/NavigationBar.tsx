import { Link } from "react-router-dom";
import logo from "../assets/images/logo.png";
import useNavBar from "../hooks/useNavBar";

export default function NavigationBar() {
    const {handleProblem, handleRegister, handleLogin, user, error, loading} = useNavBar();

    return (
        <div className="flex flex-row items-center gap-10 bg-[#0D1117] border-b px-0.5 py-0.75">
            <Link to="/" className="p-3 ml-5">
                <img className="w-7 h-7 duration-500 hover:scale-110" src={logo}></img>
            </Link>
            <Link to="/problems">
                <p className="text-gray-200 font-black text-sm px-10 py-1 duration-500 hover:bg-[#1E3A8A] rounded-2xl" onClick={handleProblem}>Problems</p>
            </Link>
            {!loading && user.userId === null && (
                <div className="ml-auto mr-10">
                    <Link to="/auth/login">
                        <p className="text-gray-200 font-black text-sm px-7 py-1 duration-500 hover:bg-[#1E3A8A] rounded-md" onClick={handleLogin}>Login</p>
                    </Link>
                </div>
                
            )}
            {!loading && user.userId !== null && (
                <p className="text-lg">Hello {user.displayName}</p>
            )}
        </div>
    )
}