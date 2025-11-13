import { useEffect, useState } from "react";
import { problemDetail } from "../api/problem";

enum Difficulty {
    Easy = 'easy',
    Medium = 'medium',
    Hard = 'hard'
}

interface Problem {
    problemId: string;
    title: string;
    slug: string;
    statementMd: string;
    difficulty: Difficulty;
    totalSubmissionCount: number;
    scoreAcceptedCount: number;
    acceptanceRate: number;
    tagList: Array<Tag>;
    testCaseList: Array<TestCase>;
}

interface Tag {
    tagId: string;
    name: string;
}

interface TestCase {
    testId: string;
    visibility: string;
    inputText: string;
    outputText: string;
    weight: number;
    problemId: string;
}

export default function useProblemDetail(slug: string) {
    const [problem, setProblem] = useState<Problem>();
    const [initLoading, setInitLoading] = useState<boolean>(false);
    const [initError, setInitError] = useState<string | null>(null);

    const handleGetProblem = async () => {
        try {
            setInitLoading(true);
            var response = await problemDetail(slug);
            console.log(response);
            setProblem(response);
        } catch (err: any) {
            setInitError(err.message || "Unexpected error");
        } finally {
            setInitLoading(false);
        }
    }

    useEffect(() => {
        handleGetProblem();
    }, [slug]);

    return { problem, initLoading, initError };
}