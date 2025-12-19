import { ProblemTag } from "./problemTag";
import { ProblemTemplate } from "./problemTemplate";
import { Submission } from "./submission";
import { TestCase } from "./testCase";

export enum Difficulty {
    Easy = 'easy',
    Medium = 'medium',
    Hard = 'hard'
}

export interface Problem {
    problemId: string;
    slug: string;
    title: string;
    statementMd: string;
    difficulty: Difficulty;
    timeLimitMs: number;
    memoryLimitKb: number;
    createdBy: string;
    createdAt: string;
    updatedAt: string | null;
    totalSubmissionCount: number;
    scoreAcceptedCount: number;
    acceptanceRate: number | null;
    isActive: boolean;
    submissions: Array<Submission>;
    testCases: Array<TestCase>;
    tags: Array<ProblemTag>;
    problemTemplates: Array<ProblemTemplate>;
}