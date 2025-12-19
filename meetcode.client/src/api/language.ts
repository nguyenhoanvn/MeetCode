import { Language } from "../types/admin/language";
import { LanguageGetRequest } from "../types/request/languageGetRequest";
import { LanguageGetResponse } from "../types/response/languageGetResponse";
import { LanguageListResponse } from "../types/response/languageListResponse";
import { LanguageUpdateRequest } from "../types/system/languageUpdateRequest";
import { languageApi } from "./client"

/* Get all languages endpoint */
export const languageList = async () => {
    const response = await languageApi.get<LanguageListResponse>("");
    return response.data.languageList;
}

/* Get by name language endpoint */
export const languageGet = async (request: LanguageGetRequest) => {
    const response = await languageApi.get<LanguageGetResponse>(`/${request.langId}`);
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