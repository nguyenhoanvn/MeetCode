import { LoginResponse, RefreshResponse } from "../../types/response/authResponses";
import { authApi } from "../client";



export const register = async (registerRequest: {displayName: string; email: string; password: string;}) => {
    const response = await authApi.post("/register", registerRequest);
    return response;
}
    

export const login = async (loginRequest: {email: string; password: string;}) =>  {
    const response = await authApi.post<LoginResponse>("/login", loginRequest);
    return response.data;
}

export const refresh = async () => {
    const response = await authApi.get<RefreshResponse>("/refresh");
    return response.data;
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

export const logout = async () => {
    const response = await authApi.post("/logout");
    return response.data;
}