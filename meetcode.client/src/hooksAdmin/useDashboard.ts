import { useState } from "react";
import { User } from "../types/admin/user";

export default function useDashboard() {
    const [user, setUser] = useState<User>();
    const [loading, setLoading] = useState<boolean>(false);
    const [error, setError] = useState<string | null>(null);
    const [pageLoading, setPageLoading] = useState<boolean>(false);

    const userGet = async () => {
        try {
            setPageLoading(true);

            

        } catch (err: unknown) {
            
        } finally {
            setPageLoading(false);
        }
    }
}