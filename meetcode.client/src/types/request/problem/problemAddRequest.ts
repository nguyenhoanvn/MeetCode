import { Difficulty } from "../../admin/problem";
import { ProblemTag } from "../../admin/problemTag";
import { ProblemTemplateAddRequest } from "../problemTemplate/problemTemplateAddRequest";

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
