import { authApi } from "./client";

export const register = async (registerRequest: {displayName: string; email: string; password: string;}) => {
    const response = await authApi.post("/register", registerRequest);
    return response;
}
    

export const login = async (loginRequest: {email: string; password: string;}) =>  {
    const response = await authApi.post("/login", loginRequest);
    return response;
}

export const refresh = async () => {
    await authApi.get("/refresh");
}

export const forgotPassword = async (forgotPasswordRequest: {email: string;}) => {
    const response = await authApi.post("/forgot-password", forgotPasswordRequest);
    return response.data;
}

export const verifyOtp = async (verifyOtpRequest: {code: string;}) => {
    const response = await authApi.post("/verify-otp", verifyOtpRequest);
    return response.data;
}

export const resetPassword = async (resetPasswordRequest: {email:string; newPassword: string;}) => {
    const response = await authApi.post("/reset-password", resetPasswordRequest);
    return response.data;
}