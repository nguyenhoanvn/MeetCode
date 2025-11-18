import { useEffect, useState } from "react";
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

    const getParsedTestCases = (): ParsedTestCase[] => {
        return testCases.map(tc => {
            let parsedInput: Record<string, any> = {};
            try {
                parsedInput = JSON.parse(tc.inputJson);
            } catch (err: any) {
                console.log("Cannot parse from json to text");
            }
            return {...tc, input: parsedInput};
        })
    }

    return {testCases, updateTestCase, removeTestCase, getParsedTestCases};
}