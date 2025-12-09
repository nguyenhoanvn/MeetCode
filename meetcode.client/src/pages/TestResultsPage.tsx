import { useTestResult } from "../hooks/useTestResult";
import { TestResult } from "../types/testResult";

interface TestResultsPageProps {
    jobId: string | null;
    results: TestResult[];
}

export default function TestResultsPage({ results }: TestResultsPageProps) {
    return (
        <div>
            {results.map(r => (
                <div key={r.testCase.testId}>
                    <p>this test case input: {r.testCase.inputJson} and produces output: {r.testCase.outputJson}</p>
                </div>
            ))}
        </div>
    );
}