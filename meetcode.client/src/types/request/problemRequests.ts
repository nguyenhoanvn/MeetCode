import { Difficulty } from "../admin/problem";

export interface ProblemOption {
    problemId: string;
    title: string;
}

export interface ProblemAddRequest {
    title: string;
    difficulty: Difficulty;
    statementMd: string;
    timeLimitMs: number;
    memoryLimitMb: number;
    createdBy: string;
    tagIds: string[];
}
