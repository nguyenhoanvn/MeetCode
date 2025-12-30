import { useEffect, useState } from "react"
import { RunCode } from "../types/runCode"
import { runCode } from "../api/user/submit";
import { TestCase } from "../types/testCase";
import { TestResult } from "../types/testResult";
import { TestJobResult } from "../types/testJobResult";
import { profileMinimal } from "../api/user/profile";

export default function useRunCode(initLanguageName: string, initProblemId: string, initCode: string, initTestCaseIds: Array<string>, onResultArrived?: () => void) {
    const [runCodeRequest, setRunCode] = useState<RunCode>({
        languageName: initLanguageName,
        userId: "",
        problemId: initProblemId,
        code: initCode,
        testCaseIds: initTestCaseIds
    });

    const [jobId, setJobId] = useState<string | null>(null);
    const [loading, setLoading] = useState<boolean>(false);
    const [results, setResults] = useState<TestResult[]>([]);
    

    const submitJob = async () => {
        setLoading(true);
        const user = await profileMinimal();

        const request: RunCode = {
            ...runCodeRequest,
            userId: user.userId
        };

        const resp = await runCode(request);
        setJobId(resp.jobId);

    };

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

    useEffect(() => {
        if (!jobId) return;

        const socket = new WebSocket("ws://localhost:8181");

        socket.onopen = () => {
            console.log("WS connected for job", jobId);
            const registrationMessage = {
                JobId: jobId,
                MessageSent: runCodeRequest
            };
            
            socket.send(JSON.stringify(registrationMessage));
            console.log("Sent registration:", registrationMessage);
        };

        socket.onmessage = (event) => {
            console.log("received: " + event.data);
            const data: TestJobResult = JSON.parse(event.data);

            if (data.jobId !== jobId) {
                console.log("JobId mismatch, ignoring");
                return;
            }

            if (!Array.isArray(data.testResults)) {
                console.log("TestResults is not an array");
                return;
            }

            const mappedResults = data.testResults.map((r: TestResult) => ({
                testCase: r.testCase,
                result: r.result,
                isSuccessful: r.isSuccessful,
                execTimeMs: r.execTimeMs
            }));
            
            console.log("Setting results to:", mappedResults);
            setResults(mappedResults);
            setLoading(false);

            onResultArrived?.();
        };

        return () => socket.close();
    }, [jobId]);

    useEffect(() => {
        console.log("Results updated:", results);
    }, [results]);


    return {
        runCodeRequest,
        handleChangeCode,
        handleChangeTestCase,
        loading,
        submitJob,
        jobId,
        results
    };
}