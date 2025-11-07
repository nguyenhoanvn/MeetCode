import { profileApi } from "./client";

export const profileMinimal = async () => {
    const response = await profileApi.get("/me");
    return response.data;
}