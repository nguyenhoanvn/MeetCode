import { authApi } from "./client";

export const register = async (registerRequest: {displayName: string; email: string; password: string;}) => {
    await authApi.post("/register", registerRequest);
}
    

export const login = async (loginRequest: {email: string; password: string;}) =>  {
    const response = await authApi.post("/login", loginRequest);
    return response.data;
}

export const refresh = async () => {
    await authApi.get("/refresh");
}

export const forgotPassword = async (forgotPasswordRequest: {email: string;}) => {
    const response = await authApi.post("/forgot-password", forgotPasswordRequest);
    return response.data;
}