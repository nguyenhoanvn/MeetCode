import { useEffect, useState } from "react";
import { User } from "../types/admin/user";
import { currentUserGet } from "../api/admin/user";

export default function useDashboard() {
    const [user, setUser] = useState<User>();
    const [loading, setLoading] = useState<boolean>(false);
    const [error, setError] = useState<string | null>(null);
    const [pageLoading, setPageLoading] = useState<boolean>(false);

    useEffect(() => {
        userGet();
    }, [])

    const userGet = async () => {
        try {
            setPageLoading(true);

            const userResponse = await currentUserGet();

            setUser(userResponse);

        } catch (err: unknown) {
            
        } finally {
            setPageLoading(false);
        }
    }

    return {
        user,
        loading,
        error,
        pageLoading
    }
}