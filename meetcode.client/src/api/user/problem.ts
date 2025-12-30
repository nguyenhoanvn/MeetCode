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
    const response = await problemApi.get<ProblemListUserReponse>(`/${slug}`);
    return response.data;
}

export const problemSearch = async (title: string) => {
    const response = await problemApi.get<ProblemListUserReponse>(`/search?title=${title}`);
    return response.data;
}