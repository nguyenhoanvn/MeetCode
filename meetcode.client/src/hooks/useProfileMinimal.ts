import { useEffect, useState } from "react";
import { ProfileUserMinimal } from "../types/response/profileResponses";
import { profileMinimal } from "../api/user/profile";

export default function useProfileMinimal() {
    const [user, setUser] = useState<ProfileUserMinimal>();

    useEffect(() => {
        fetchProfile();
    }, []);

    const fetchProfile = async () => {
        const response = await profileMinimal();
        setUser(response);
    };

    return {
        user
    };
}