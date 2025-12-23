import { ProblemTemplateListResponse } from "../types/response/problemTemplateListResponse";
import { ProblemTemplateResponse } from "../types/response/problemTemplateResponse"
import { problemTemplateApi } from "./client"

/* Get all templates endpoint */
export const problemTemplateList = async () => {
    const response = await problemTemplateApi.get<ProblemTemplateListResponse>("");
    return response.data.templateList;
}

/* Get template by id */
export const problemTemplateGet = async (id: string) => {
    const response = await problemTemplateApi.get<ProblemTemplateResponse>(`/${id}`);
    return response.data.problemTemplate;
}

/* Toggle template status */
export const problemTemplateToggle = async (id: string) => {
    const response = await problemTemplateApi.patch<ProblemTemplateResponse>(`/${id}/toggle`);
    return response.data.problemTemplate;
}