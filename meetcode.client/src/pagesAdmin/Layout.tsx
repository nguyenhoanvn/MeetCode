import { Outlet } from "react-router-dom";
import AdminSidebar from "../components/AdminSidebar";

export default function AdminLayout() {
    return (
        <div className="h-screen w-screen grid grid-cols-[20%_80%] overflow-x-hidden">
            <AdminSidebar />
            <div className="flex-1">
                <Outlet />
            </div>
        </div>
    );
}