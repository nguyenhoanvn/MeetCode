import { useEffect, useState } from "react";
import { Language } from "../types/admin/language";
import { languageList } from "../api/admin/language";
import { ApiProblemDetail } from "../types/system/apiProblemDetail";

export default function useLanguageList() {
    const [languages, setLanguages] = useState<Language[]>([]);
    const [error, setError] = useState<string | null>(null);
    const [loading, setLoading] = useState<boolean>(false);

    useEffect(() => {
        listLanguages();
    }, []);

    const listLanguages = async () => {
        try {
            setLoading(true);
            setError(null);

            const languageResponse = await languageList();
            
            setLanguages(languageResponse);
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
        languages,
        error,
        loading
    };
}