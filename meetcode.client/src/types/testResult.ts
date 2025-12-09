import { TestCase } from "./testCase";

export interface TestResult {
    testCase: TestCase;
    result: string;
    isSuccessful: boolean;
    execTimeMs: number;
};