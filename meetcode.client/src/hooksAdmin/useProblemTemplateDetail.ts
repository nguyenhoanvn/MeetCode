import { useEffect, useState } from "react";
import { ProblemTemplate } from "../types/admin/problemTemplate";
import { problemTemplateGet, problemTemplateToggle } from "../api/problemTemplate";
import { ApiProblemDetail } from "../types/system/apiProblemDetail";

export default function useProblemTemplateDetail(id: string) {
    const [problemTemplate, setProblemTemplate] = useState<ProblemTemplate>();
    const [loading, setLoading] = useState<boolean>(false);
    const [error, setError] = useState<string | null>(null);
    const [toggleLoading, setToggleLoading] = useState<boolean>(false);

    useEffect(() => {
        getProblemTemplate();
    }, [id]);

    const getProblemTemplate = async () => {
        try {
            setLoading(true);
            setError(null);

            const problemTemplate = await problemTemplateGet(id);

            setProblemTemplate(problemTemplate);
        } catch (err: unknown) {
            const apiError = err as ApiProblemDetail
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
            setError(null);

            const templateResponse = await problemTemplateToggle(problemTemplate?.templateId ?? "");

            setProblemTemplate(templateResponse);
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
        problemTemplate,
        loading,
        error,
        toggleStatus,
        toggleLoading
    };
}