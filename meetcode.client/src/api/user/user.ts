import { UserSubmissionProblemGet } from "../../types/request/userRequests";
import { SubmissionAllResponse } from "../../types/response/submissionResponses";
import { userApi } from "../client";

export const submissionGet = async (submissionGetRequest: UserSubmissionProblemGet) => {
    const response = await userApi.get<SubmissionAllResponse>(`/${submissionGetRequest.userId}/problems/${submissionGetRequest.problemId}/submissions`);
    return response.data;
}