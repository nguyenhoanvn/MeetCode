import { useEffect, useState } from "react";
import { Problem } from "../types/admin/problem";
import { problemGet, problemToggle } from "../api/admin/problem";
import { ApiProblemDetail } from "../types/system/apiProblemDetail";

export default function useProblemDetail(id: string) {
    const [problem, setProblem] = useState<Problem>();
    const [error, setError] = useState<string | null>(null);
    const [loading, setLoading] = useState<boolean>(false);
    const [toggleLoading, setToggleLoading] = useState<boolean>(false);

    useEffect(() => {
        handleGetProblem();
    }, [id]);

    const handleGetProblem = async () => {
        try {
            setLoading(true);

            const problemResponse = await problemGet(id);

            setProblem(problemResponse);
        } catch (err: unknown) {
            const apiError = err as ApiProblemDetail;
            if (apiError.errors) {
                const entries = Object.entries(apiError.errors ?? {});
                if (entries.length > 0) {
                    const [field, messages] = entries[0];
                    setError(messages[0]);
                }
            } else {
                setError("Unknown error");
            }
        } finally {
            setLoading(false);
        }
    }

    const toggleStatus = async () => {
        try {
            setToggleLoading(true);

            const problemResponse = await problemToggle(id);

            setProblem(problemResponse);
        } catch (err: unknown) {
            const apiError = err as ApiProblemDetail;
            if (apiError.errors) {
                const entries = Object.entries(apiError.errors ?? {});
                if (entries.length > 0) {
                    const [field, messages] = entries[0];
                    setError(messages[0]);
                }
            } else {
                setError("Unknown error");
            }
        } finally {
            setToggleLoading(false);
        }
    }
    
    return {
        problem,
        loading,
        error,
        toggleLoading,
        toggleStatus
    };
}