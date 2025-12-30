export interface TestCase {
    testId: string;
    visibility: string;
    inputJson: string;
    output: string;
    problemId: string;
}

export interface ParsedTestCase extends Omit<TestCase, "inputJson"> {
    input: Record<string, string>;
}