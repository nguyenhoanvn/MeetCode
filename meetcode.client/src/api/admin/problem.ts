import { ProblemListResponse } from "../../types/response/problem/problemListResponse";
import { ProblemResponse } from "../../types/response/problem/problemResponse";
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