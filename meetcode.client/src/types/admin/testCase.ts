import { Problem } from "./problem";

export enum Visibility {
    Sample = 'sample',
    Hidden = 'hidden',
    Public = 'public'
}

export interface TestCase {
    testId: string;
    problemId: string;
    visibility: Visibility;
    inputText: string;
    inputJson: string;
    expectedOutputText: string;
    weight: string;
    problem: Problem;
}