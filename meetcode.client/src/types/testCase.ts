export enum Visibility {
    Sample = 'sample',
    Hidden = 'hidden',
    Public = 'public'
}

export interface TestCase {
    testId: string;
    visibility: Visibility;
    inputJson: string;
    outputJson: string;
    weight: number;
    problemId: string;
}

export interface ParsedTestCase extends Omit<TestCase, "inputJson"> {
    input: Record<string, any>;
}