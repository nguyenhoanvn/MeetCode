import { ParsedTestCase, TestCase } from "./testCase";

export interface TestResult {
    testCase: TestCase;
    result: string;
    isSuccessful: boolean;
    execTimeMs: number;
};

export interface ParsedTestResult extends Omit<TestResult, "testCase"> {
    testCase: ParsedTestCase;
}