import { ProblemTemplateAddRequest } from "../../types/request/problemTemplateRequests";
import { ProblemTemplateListResponse, ProblemTemplateResponse } from "../../types/response/problemTemplateResponses";
import { adminProblemTemplateApi } from "../client"

/* Get all templates endpoint */
export const problemTemplateList = async () => {
    const response = await adminProblemTemplateApi.get<ProblemTemplateListResponse>("");
    return response.data.templateList;
}

/* Get template by id */
export const problemTemplateGet = async (id: string) => {
    const response = await adminProblemTemplateApi.get<ProblemTemplateResponse>(`/${id}`);
    return response.data.problemTemplate;
}

/* Toggle template status */
export const problemTemplateToggle = async (id: string) => {
    const response = await adminProblemTemplateApi.patch<ProblemTemplateResponse>(`/${id}/toggle`);
    return response.data.problemTemplate;
}

export const problemTemplateAdd = async (request: ProblemTemplateAddRequest) => {
    const repsonse = await adminProblemTemplateApi.post<ProblemTemplateResponse>("", request);
    return repsonse.data.problemTemplate;
}