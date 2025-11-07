import axios from "axios";
import { enableUserAuthInterceptor } from "./interceptors";

export const authApi = axios.create({
    baseURL: "https://localhost:7254/auth",
    withCredentials: true,
    headers: {"Content-Type": "application/json"},
});

export const profileApi = axios.create({
    baseURL: "https://localhost:7254/profile",
    withCredentials: true,
    headers: {"Content-Type": "application/json"},
});

enableUserAuthInterceptor(authApi);
enableUserAuthInterceptor(profileApi);