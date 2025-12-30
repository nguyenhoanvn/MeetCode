import { ProfileUserMinimal } from "../../types/response/profileResponses";
import { profileApi } from "../client";

export const profileMinimal = async () => {
    const response = await profileApi.get<ProfileUserMinimal>("/me");
    return response.data;
}