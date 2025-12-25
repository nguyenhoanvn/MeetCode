import { ProblemAddRequest } from "../../types/request/problemRequests";
import { ProblemListResponse, ProblemResponse } from "../../types/response/problemResponses";
import { adminProblemApi } from "../client"

/* Get all problem endpoint */
export const problemList = async () => {
    const response = await adminProblemApi.get<ProblemListResponse>("");
    return response.data.problemList;
} 

/* Get by problem Id endpoint */
export const problemGet = async (id: string) => {
    const response = await adminProblemApi.get<ProblemResponse>(`/${id}`);
    return response.data.problem;
}

/* Toggle status problem endpoint */
export const problemToggle = async (id: string) => {
    const response = await adminProblemApi.patch<ProblemResponse>(`/${id}/toggle`);
    return response.data.problem;
}

/* Add problem endpoint */
export const problemAdd = async (request: ProblemAddRequest) => {
    const response = await adminProblemApi.post<ProblemResponse>("", request);
    return response.data.problem;
}