import { Language } from "../types/admin/language";
import { LanguageGetRequest } from "../types/request/languageGetRequest";
import { LanguageResponse } from "../types/response/languageResponse";
import { LanguageListResponse } from "../types/response/languageListResponse";
import { LanguageUpdateRequest } from "../types/system/languageUpdateRequest";
import { languageApi } from "./client"

/* Get all languages endpoint */
export const languageList = async () => {
    const response = await languageApi.get<LanguageListResponse>("");
    return response.data.languageList;
}

/* Get by name language endpoint */
export const languageGet = async (langId: string) => {
    const response = await languageApi.get<LanguageResponse>(`/${langId}`);
    return response.data.language;
}

/* Update language endpoint */
export const languageUpdate = async (
    langId: string,
    request: LanguageUpdateRequest
) => {
    const response = await languageApi.patch(`/${langId}`, request);

    return response.data;
}

/* Toggle status language endpoint */
export const languageStatusToggle = async (langId: string) => {
    const response = await languageApi.patch<LanguageResponse>(`/${langId}/toggle`);
    return response.data.language;
}