import { useEffect, useMemo, useState } from "react";
import { TestCase, ParsedTestCase } from "../types/testCase";

export default function useTestCaseList(initialList: Array<TestCase>) {
    const [testCases, setTestCases] = useState<Array<TestCase>>([]);

    useEffect(() => {
        setTestCases(initialList);
    }, [initialList]);

    const updateTestCase = (testId: string, updated: Partial<TestCase>) => {
        setTestCases(prev =>
            prev.map(tc => tc.testId === testId ? { ...tc, ...updated } : tc)
        );
    };

    const removeTestCase = (testId: string) => {
        setTestCases(prev => prev.filter(tc => tc.testId !== testId));
    };

    const parsedTestCase: ParsedTestCase[] = useMemo(() => {
        return testCases.map(tc => {
            let input: Record<string, string> = {};
            try {
                input = JSON.parse(tc.inputJson);
            } catch (err: any) {
                console.warn("Cannot parse inputJson for test cases", tc.testId);
            }
            return { ...tc, input };
        });
    }, [testCases]);

    return {testCases, updateTestCase, removeTestCase, parsedTestCase};
}