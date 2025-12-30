import { Difficulty } from "../admin/problem";
import { ProblemTemplate } from "./problemTemplate";
import { ProblemTag } from "./tag";
import { TestCase } from "./testCase";

export interface Problem {
    problemId: string;
    slug: string;
    title: string;
    statementMd: string;
    difficulty: Difficulty;
    totalSubmissionCount: number;
    scoreAcceptedCount: number;
    acceptanceRate: number | null;
    testCases: TestCase[];
    tags: ProblemTag[];
    problemTemplates: ProblemTemplate[];
}