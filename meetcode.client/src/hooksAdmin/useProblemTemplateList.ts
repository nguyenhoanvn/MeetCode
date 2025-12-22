import { useEffect, useState } from "react";
import { ProblemTemplate } from "../types/admin/problemTemplate";
import { problemTemplateList } from "../api/problemTemplate";
import { ApiProblemDetail } from "../types/system/apiProblemDetail";

export default function useProblemTemplateList() {
    const [problemTemplates, setProblemTemplates] = useState<ProblemTemplate[]>([]);
    const [loading, setLoading] = useState<boolean>(false);
    const [error, setError] = useState<string | null>(null);

    useEffect(() => {
        listTemplates();
    }, []);

    const listTemplates = async () => {
        try {
            setLoading(true);
            setError(null);

            const templateResponse = await problemTemplateList();

            setProblemTemplates(templateResponse);
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

    return {
        problemTemplates,
        loading,
        error
    };
}