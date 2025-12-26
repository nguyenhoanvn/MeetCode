import { useEffect, useState } from "react";
import { Link, Outlet } from "react-router-dom";
import { isAdmin } from "./adminCheck";

export default function AdminGuard() {

    useEffect(() => {
        const check = async () => {
            const ok = await isAdmin();
        };
        check();
    }, []);

    return <Outlet />;
}