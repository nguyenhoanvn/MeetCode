import { useEffect, useState } from "react";
import { Problem } from "../types/problem";

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