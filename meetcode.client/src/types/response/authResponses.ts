import { User } from "../admin/user";

export interface LoginResponse {
    user: User;
    accessToken: string;
}

export interface RefreshResponse {
    jwt: string;
}