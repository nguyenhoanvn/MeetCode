import { useEffect, useState } from "react"
import { RunCode } from "../types/runCode"
import { runCode } from "../api/submit";
import { TestCase } from "../types/testCase";
import { TestResult } from "../types/testResult";

export default function useRunCode(initLanguageName: string, initProblemId: string, initCode: string, initTestCaseIds: Array<string>) {
    const [runCodeRequest, setRunCode] = useState<RunCode>({
        languageName: initLanguageName,
        problemId: initProblemId,
        code: initCode,
        testCaseIds: initTestCaseIds
    });

    const [jobId, setJobId] = useState<string | null>(null);
    const [loading, setLoading] = useState<boolean>(false);
    const [results, setResults] = useState<TestResult[]>([]);
    

    const submitJob = async () => {
        setLoading(true);
        setResults([]);

        try {
            const resp = await runCode(runCodeRequest);
            setJobId(resp.jobId);
        } finally {
            setLoading(false);
        }
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
            const data = JSON.parse(event.data);
            console.log("WS message:", data);
            console.log("Message TestResults:", data.TestResults);
            console.log("Is array?", Array.isArray(data.TestResults));
            console.log("JobId match?", data.JobId, "===", jobId, data.JobId === jobId);

            if (data.JobId !== jobId) {
                console.log("JobId mismatch, ignoring");
                return;
            }

            if (!Array.isArray(data.TestResults)) {
                console.log("TestResults is not an array");
                return;
            }

            const mappedResults = data.TestResults.map((r: any) => ({
                testCaseId: r.TestCaseId,
                result: r.Result,
                isSuccessful: r.IsSuccessful,
                execTimeMs: r.ExecTimeMs
            }));
            
            console.log("Setting results to:", mappedResults);
            setResults(mappedResults);
            setLoading(false);
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