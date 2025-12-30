import { ProblemListUserReponse, ProblemUserReponse } from "../../types/response/problemResponses";
import { Problem } from "../../types/user/problem";
import { problemApi } from "../client";

export const problemList = async (pageNumber: number, pageSize: number) => {
    const response = await problemApi.get<ProblemListUserReponse>("", {
        params: { pageNumber, pageSize }
    });
    return response.data;
} 

export const problemGet = async (slug: string) => {
    const response = await problemApi.get<Problem>(`/${slug}`);
    return response.data;
}