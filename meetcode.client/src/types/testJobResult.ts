import { TestResult } from "./testResult";

export interface TestJobResult {
    jobId: string;
    status: string;
    testResults: Array<TestResult>;
}