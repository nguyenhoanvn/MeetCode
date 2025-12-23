
import { LanguageListResponse } from "../../types/response/language/languageListResponse";
import { LanguageUpdateRequest } from "../../types/request/language/languageUpdateRequest";
import { adminLanguageApi } from "../client"
import { LanguageResponse } from "../../types/response/language/languageResponse";

/* Get all languages endpoint */
export const languageList = async () => {
    const response = await adminLanguageApi.get<LanguageListResponse>("");
    return response.data.languageList;
}

/* Get by name language endpoint */
export const languageGet = async (langId: string) => {
    const response = await adminLanguageApi.get<LanguageResponse>(`/${langId}`);
    return response.data.language;
}

/* Update language endpoint */
export const languageUpdate = async (
    langId: string,
    request: LanguageUpdateRequest
) => {
    const response = await adminLanguageApi.patch(`/${langId}`, request);

    return response.data;
}

/* Toggle status language endpoint */
export const languageStatusToggle = async (langId: string) => {
    const response = await adminLanguageApi.patch<LanguageResponse>(`/${langId}/toggle`);
    return response.data.language;
}