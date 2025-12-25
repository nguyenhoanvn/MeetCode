import { Difficulty } from "../admin/problem";

export type TagOption = {
    tagId: string;
    name: string;
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
