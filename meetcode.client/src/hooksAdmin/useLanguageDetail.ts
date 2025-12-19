import { useEffect, useState } from "react";
import { Language } from "../types/admin/language";
import { languageGet } from "../api/language";
import { LanguageGetRequest } from "../types/request/languageGetRequest";
import { ApiProblemDetail } from "../types/system/apiProblemDetail";

export default function useLanguageDetail(id: string) {
    const [language, setLanguage] = useState<Language>();
    const [loading, setLoading] = useState<boolean>(false);
    const [error, setError] = useState<string | null>(null);

    const handleGetLanguage = async () => {
        try {
            setLoading(true);

            const request: LanguageGetRequest = {
                langId: id
            };
            const problemResponse = await languageGet(request);

            setLanguage(problemResponse);
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
        handleGetLanguage();
    }, [id]);

    return {
        language,
        loading,
        error
    }
}