import { useEffect, useMemo, useState } from "react";
import { TestCase, ParsedTestCase } from "../types/testCase";

export default function useTestCaseList(initialList: Array<TestCase>) {
    const [testCases, setTestCases] = useState<Array<TestCase>>([]);
    const [selectedTab, setSelectedTab] = useState(0);

    useEffect(() => {
        setTestCases(initialList);
        setSelectedTab(0); 
    }, [initialList]);

    const updateTestCase = (testId: string, updated: Partial<TestCase>) => {
        setTestCases(prev =>
            prev.map(tc => tc.testId === testId ? { ...tc, ...updated } : tc)
        );
    };

    const removeTestCase = (testId: string) => {
        setTestCases(prev => {
            const newList = prev.filter(tc => tc.testId !== testId);
            
            if (prev[selectedTab]?.testId === testId) {
                setSelectedTab(0);
            }
            else if (selectedTab >= newList.length) {
                setSelectedTab(newList.length - 1);
            }

            return newList;
        });
    };

    const parsedTestCase: ParsedTestCase[] = useMemo(() => {
        return testCases.map(tc => {
            let input: Record<string, string> = {};
            try {
                input = JSON.parse(tc.inputJson);
            } catch {
                console.warn("Cannot parse inputJson for test cases", tc.testId);
            }
            return { ...tc, input };
        });
    }, [testCases]);

    return {testCases, updateTestCase, removeTestCase, parsedTestCase, selectedTab, setSelectedTab };
}
