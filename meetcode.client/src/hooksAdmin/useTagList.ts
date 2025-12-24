import { useEffect, useState } from "react";
import { ProblemTag } from "../types/admin/problemTag";
import { ApiProblemDetail } from "../types/system/apiProblemDetail";
import { tagList } from "../api/admin/tag";

export default function useTagList() {
    const [tags, setTags] = useState<ProblemTag[]>([]);
    const [loading, setLoading] = useState<boolean>(false);
    const [error, setError] = useState<string | null>(null);

    useEffect(() => {
        listTags();
    }, []);

    const listTags = async () => {
        try {
            setLoading(true);

            const tagResponse = await tagList();

            setTags(tagResponse);
        } catch (err: unknown) {
            const apiError = err as ApiProblemDetail;
            if (apiError.errors) {
                const entries = Object.entries(apiError.errors);
                if (entries.length > 0) {
                    const [field, message] = entries[0];
                    setError(message[0]);
                }
            } else {
                setError("Unknown error");
            }
        } finally {
            setLoading(false);
        }
    }


    return {
        tags,
        loading,
        error
    };
}