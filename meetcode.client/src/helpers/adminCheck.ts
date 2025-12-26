import { jwtDecode } from "jwt-decode"
import { currentUserGet } from "../api/admin/user";
import axios from "axios";

type JwtPayload = {
    role?: string;
    roles?: string[];
};

export const isAdmin = async (): Promise<boolean> => {
    const user = await currentUserGet();
    return user.role === "admin" || user.role === "moderator";
};

