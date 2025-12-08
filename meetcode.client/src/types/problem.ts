import { TestCase } from "./testCase";
import { Tag } from "./tag";
import { ProblemTemplate } from "./problemTemplate";

export enum Difficulty {
    Easy = 'easy',
    Medium = 'medium',
    Hard = 'hard'
}

export interface Problem {
    problemId: string;
    title: string;
    slug: string;
    statementMd: string;
    difficulty: Difficulty;
    totalSubmissionCount: number;
    scoreAcceptedCount: number;
    acceptanceRate: number;
    tagList: Array<Tag>;
    testCaseList: Array<TestCase>;
    templateList: Array<ProblemTemplate>
}