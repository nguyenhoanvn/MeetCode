import { ProblemTag } from "./problemTag";
import { ProblemTemplate } from "./problemTemplate";
import { Submission } from "./submission";
import { TestCase } from "./testCase";

export type Difficulty = 'easy' | 'medium' | 'hard';

export const difficultyStyle: Record<Difficulty, string> = {
  easy: "text-emerald-300",
  medium: "text-yellow-300",
  hard: "text-red-300",
};

export interface Problem {
    problemId: string;
    slug: string;
    title: string;
    statementMd: string;
    difficulty: Difficulty;
    timeLimitMs: number;
    memoryLimitMb: number;
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