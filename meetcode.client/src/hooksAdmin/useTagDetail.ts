import { useEffect, useState } from "react";
import { ProblemTag } from "../types/admin/problemTag";
import { tagGet } from "../api/admin/tag";
import { ApiProblemDetail } from "../types/system/apiProblemDetail";

export default function useTagDetail(id: string) {
    const [tag, setTag] = useState<ProblemTag>();
    const [loading, setLoading] = useState<boolean>(false);
    const [error, setError] = useState<string | null>(null);

    const getTag = async () => {
        try {
            setLoading(true);
            setError(null);

            const tagResponse = await tagGet(id);

            setTag(tagResponse);
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

    useEffect(() => {
        getTag();
    }, [id])

    return {
        tag,
        loading,
        error
    };
}