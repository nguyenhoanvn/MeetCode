import { useEffect, useState } from "react"
import { RunCode } from "../types/runCode"
import { runCode } from "../api/submit";
import { TestCase } from "../types/testCase";

export default function useRunCode(initLanguageId: string, initProblemId: string, initCode: string, initTestCaseIds: Array<string>) {
    const [runCodeRequest, setRunCode] = useState<RunCode>({
        languageId: initLanguageId,
        problemId: initProblemId,
        code: initCode,
        testCaseIds: initTestCaseIds
    });
    const [loading, setLoading] = useState<boolean>(false);
    
    const submitJob = async () => {
        setLoading(true);
        try {
            const resp = await runCode(runCodeRequest);
            console.log(resp);
        } catch (err: any) {
            console.error(err);
        } finally {
            setLoading(false);
        }
    }

    const handleChangeCode = (value?: string) => {
        setRunCode(prev => ({
            ...prev,
            code: value ?? ""
        }));
    };

    useEffect(() => {
        setRunCode(prev => ({
            ...prev,
            code: initCode
        }));
    }, [initCode]);

    const handleChangeTestCase = (updatedTestCases: TestCase[]) => {
        setRunCode(prev => ({
            ...prev,
            testCaseIds: updatedTestCases.map(tc => tc.testId)
        }));
    };

    return {
        runCodeRequest,
        handleChangeCode,
        handleChangeTestCase,
        loading,
        submitJob
    };
}