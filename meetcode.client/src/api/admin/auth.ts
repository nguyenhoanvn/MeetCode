import { LoginResponse } from "../../types/response/authResponses";
import { adminAuthApi } from "../client";

export const login = async (loginRequest: {email: string; password: string;}) =>  {
    const response = await adminAuthApi.post<LoginResponse>("/login", loginRequest);
    return response;
}