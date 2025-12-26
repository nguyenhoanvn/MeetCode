import { UserResponse } from "../../types/response/userResponses"
import { adminMiscApi } from "../client"

/* Get current user endpoint */
export const currentUserGet = async () => {
    const response = await adminMiscApi.get<UserResponse>("/status");
    return response.data.user;
}