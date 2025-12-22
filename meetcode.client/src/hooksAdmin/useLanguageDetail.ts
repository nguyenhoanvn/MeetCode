import { useEffect, useState } from "react";
import { Language } from "../types/admin/language";
import { languageGet, languageStatusToggle } from "../api/language";
import { ApiProblemDetail } from "../types/system/apiProblemDetail";

export default function useLanguageDetail(id: string) {
    const [language, setLanguage] = useState<Language>();
    const [loading, setLoading] = useState<boolean>(false);
    const [error, setError] = useState<string | null>(null);
    const [toggleLoading, setToggleLoading] = useState<boolean>(false);

    const handleGetLanguage = async () => {
        try {
            setLoading(true);

            const languageResponse = await languageGet(id);

            setLanguage(languageResponse);
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
            
            const languageResponse = await languageStatusToggle(language?.langId ?? "");

            setLanguage(languageResponse);
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

    useEffect(() => {
        handleGetLanguage();
    }, [id]);

    return {
        language,
        loading,
        error,
        toggleLoading,
        toggleStatus
    }
}