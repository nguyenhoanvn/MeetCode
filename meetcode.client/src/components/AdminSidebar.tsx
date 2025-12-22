import { Link } from "react-router-dom";
import logo from "../assets/images/logo.png";

export default function AdminSidebar() {
    const navList: Array<{ label: string; url: string }> = [
        { label: "Dashboard", url: "" },
        { label: "Problems", url: "problems" },
        { label: "Languages", url: "languages"},
        { label: "Problem Templates", url: "problemTemplates"}
    ];

    return (
        <>
            <div className="flex flex-col border-r">
                <div className="flex px-10 py-3 gap-3 items-center border-b">
                    <div>
                        <img className="w-10 h-10" src={logo}></img>
                    </div>
                    <div className="flex flex-col">
                        <span className="font-black text-md">MeetCode</span>
                        <span className="font-black text-md text-blue-400">Admin Dashboard</span>
                    </div>
                </div>
                {navList.map(item => (
                    <div className="">
                        <Link
                            key={item.url}
                            to={item.url}
                            className="px-10 py-3 hover:bg-blue-700 block"
                        >
                            {item.label}
                        </Link>
                    </div>
                ))}
            </div>
        </>
    )
}