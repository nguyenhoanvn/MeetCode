import { useEffect, useMemo, useState } from "react";
import { ParsedTestResult, TestResult } from "../types/testResult";
import { ParsedTestCase } from "../types/testCase";

export default function useTestResult(initial: Array<TestResult>) {
    const [results, setResults] = useState<Array<TestResult>>(initial ?? []);
    const [selectedTab, setSelectedTab] = useState<number>(0);

    const parsedTestCase: ParsedTestResult[] = useMemo(() => {
        return results.map(r => {
            let input: Record<string, string> = {};
            let output: string;

            try {
                input = JSON.parse(r.testCase.inputJson);
            } catch (err: any) {
                console.log("Something went wrong while parsing: " + err)
            }

            const parsedTestCase: ParsedTestCase =  {
                ...r.testCase,
                input
            };

            return {...r, testCase: parsedTestCase};
        });
    }, [results]);

    useEffect(() => {
        setResults(initial ?? []);
        setSelectedTab(0);
    }, [initial]);

    const safeSelectedTab =
        results.length === 0 ? 0 :
        selectedTab >= results.length ? 0 :
        selectedTab;

    return { results, parsedTestCase, selectedTab: safeSelectedTab, setSelectedTab };
}