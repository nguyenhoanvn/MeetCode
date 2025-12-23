import { useEffect, useState } from "react";
import { problemList } from "../api/admin/problem";
import { Problem } from "../types/admin/problem";
import { ApiProblemDetail } from "../types/system/apiProblemDetail";

export default function useProblemList() {
    const [problems, setProblems] = useState<Problem[]>([]);
    const [loading, setLoading] = useState<boolean>(false);
    const [error, setError] = useState<string | null>(null);

    useEffect(() => {
        listProblems();
    }, []);

    const listProblems = async () => {
        try {
            setLoading(true);
            setError(null);

            const problemResponse = await problemList();

            setProblems(problemResponse);
        } catch (err: unknown) {
            const apiError = err as ApiProblemDetail;
            if (apiError.errors) {
                const entries = Object.entries(apiError.errors);
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

    return {
        problems,
        loading,
        error
    };
}