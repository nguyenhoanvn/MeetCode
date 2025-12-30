import { useEffect, useState } from "react";
import { ProblemAddRequest } from "../types/request/problemRequests";
import { problemAdd } from "../api/admin/problem";
import { ApiProblemDetail } from "../types/system/apiProblemDetail";
import { ProblemTag } from "../types/admin/problemTag";
import { tagSearch } from "../api/admin/tag";
import { Difficulty } from "../types/admin/problem";
import { TagOption } from "../types/request/tagRequests";

export interface ProblemAddForm {
    title: string;
    difficulty: Difficulty;
    statementMd: string;
    timeLimitMs: number;
    memoryLimitMb: number;
    createdBy: string;
    tags: TagOption[];
}


export default function useProblemAdd() {
    const [problemAddForm, setProblemAddForm] = useState<ProblemAddForm>({
        title: "",
        difficulty: "easy",
        statementMd: "",
        timeLimitMs: 0,
        memoryLimitMb: 0,
        createdBy: "",
        tags: [] as TagOption[]
    });
    const [pageLoading, setPageLoading] = useState<boolean>(false);
    const [success, setSuccess] = useState<string | null>(null);
    const [loading, setLoading] = useState<boolean>(false);
    const [tagSearchLoading, setTagSearchLoading] = useState<boolean>(false);
    const [error, setError] = useState<string | null>(null);
    const [errorField, setErrorField] = useState<string | null>(null);
    const [previewStatementMd, setPreviewStatementMd] = useState<boolean>(false);
    const [tagNameQuery, setTagNameQuery] = useState<string>("");
    const [suggestions, setSuggestions] = useState<TagOption[]>([]);

    const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
        setError(null);
        const { name, value } = e.target;
        setProblemAddForm((prev) => ({ ...prev, [name]: value }));
    }

    const handleTagDelete = (tagId: string) => {
        setProblemAddForm((prev) => ({
            ...prev,
            tags: prev.tags.filter(t => t.tagId !== tagId)
        }))
    }

    const handleTagAdd = (tag: TagOption) => {
        setProblemAddForm((prev) => {
            if (prev.tags.some(t => t.tagId === tag.tagId)) {
                return prev;
            }

            return {
                ...prev,
                tags: [...prev.tags, tag],
            };
        });
        setTagNameQuery("");
    };

    const validate = () => {
        if (!problemAddForm.title) {
            setError("Problem title is required")
            setErrorField("title");
            return false;
        }
        if (!problemAddForm.timeLimitMs || problemAddForm.timeLimitMs <= 0) {
            setError("Time limit must be a positive integer")
            setErrorField("timeLimitMs");
            return false;
        }
        if (!problemAddForm.memoryLimitMb || problemAddForm.timeLimitMs <= 0) {
            setError("Memory limit must be a positive integer")
            setErrorField("memoryLimitMb");
            return false;
        }
        if (problemAddForm.tags.length === 0) {
            setError("You must add at least one tag");
            setErrorField("tag");
            return false;
        }
        if (!problemAddForm.statementMd) {
            setError("StatementMd is required");
            setErrorField("statementMd");
            return false;
        }
        return true;
    }

    const handleSubmit = async () => {
        try {
            setLoading(true);
            setError(null);
            setSuccess(null);

            if (!validate()) {
                return;
            }

            const payload: ProblemAddRequest = {
                title: problemAddForm.title,
                difficulty: problemAddForm.difficulty,
                statementMd: problemAddForm.statementMd,
                timeLimitMs: problemAddForm.timeLimitMs,
                memoryLimitMb: problemAddForm.memoryLimitMb,
                createdBy: problemAddForm.createdBy,
                tagIds: problemAddForm.tags.map(t => t.tagId),
            };

            const problemResponse = await problemAdd(payload);

            setSuccess(`Problem created with Id ${problemResponse.problemId}`);

        } catch (err: unknown) {
            const apiError = err as ApiProblemDetail;
            if (apiError.errors) {
                const entries = Object.entries(apiError.errors);
                if (entries.length > 0) {
                    const [field, message] = entries[0];
                    setErrorField(field);
                    setError(message[0]);
                }
            } else if (apiError.status === 401) {
                setError("You are not logged in");
            } else {
                setError("Unknown error");
            }
        } finally {
            setLoading(false);
        }
    }

    const handleTagSuggestion = async () => {
        if (!tagNameQuery) {
            setSuggestions([]);
            return;
        }

        const timeout = setTimeout(async () => {
            try {
                setTagSearchLoading(true);

                const tagResponse = await tagSearch(tagNameQuery);

                setSuggestions(
                    tagResponse.map((t) => ({
                        tagId: t.tagId,
                        name: t.name,
                    }))
                );

            } catch (err: unknown) {
                const apiError = err as ApiProblemDetail;
                if (apiError.errors) {
                    const entries = Object.entries(apiError.errors);
                    if (entries.length > 0) {
                        const [field, message] = entries[0];
                        setError(message[0]);
                        setErrorField(field);
                    }
                } else {
                    setError("Unknown error");
                }
            } finally {
                setTagSearchLoading(false);
            }
        }, 300);

        return () => clearTimeout(timeout);
    }

    const handleQueryChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
        setError(null);
        const { name, value } = e.target;
        setTagNameQuery(value);
    }

    const togglePreview = () => {
        setPreviewStatementMd(!previewStatementMd);
    }

    useEffect(() => {
        handleTagSuggestion()
    }, [tagNameQuery]);

    return {
        problemAddForm,
        pageLoading,
        tagNameQuery,
        handleChange,
        handleSubmit,
        togglePreview,
        previewStatementMd,
        handleQueryChange,
        handleTagDelete,
        handleTagAdd,
        loading,
        suggestions,
        error,
        success,
        errorField
    };
}