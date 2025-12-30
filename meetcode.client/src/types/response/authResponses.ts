import { User } from "../admin/user";

export interface LoginResponse {
    user: User;
    accessToken: string;
}