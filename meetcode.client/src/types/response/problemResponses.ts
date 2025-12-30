import { Problem } from "../admin/problem";
import { Problem as ProblemDto } from "../user/problem";

export interface ProblemResponse {
    problem: Problem;
}

export interface ProblemListResponse {
    problemList: Problem[];
}

export interface ProblemUserReponse {
    problem: ProblemDto
}

export interface ProblemListUserReponse {
    pageNumber: number;
    pageSize: number;
    problemList: ProblemDto[];
    totalCount: number;
    totalPages: number;
}