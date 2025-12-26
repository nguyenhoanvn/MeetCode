import { useEffect, useMemo, useState } from "react";
import { ProblemTemplateAddRequest } from "../types/request/problemTemplateRequests";
import { problemTemplateAdd } from "../api/admin/problemTemplate";
import { ApiProblemDetail } from "../types/system/apiProblemDetail";
import { Problem } from "../types/admin/problem";
import { Language } from "../types/admin/language";
import { problemList } from "../api/admin/problem";
import { languageList } from "../api/admin/language";
import { Variable } from "../types/system/variable";

export interface ProblemTemplateAddForm {
    problemId: string;
    langId: string;
    methodName: string;
    returnType: string;
    parameters: Variable[];
    compileCommand: string | null;
    runCommand: string | null;
}

export default function useProblemTemplateAdd() {
    const [problemTemplateAddForm, setProblemTemplateAddForm] = useState<ProblemTemplateAddForm>({
        problemId: "",
        langId: "",
        methodName: "",
        returnType: "",
        parameters: [],
        compileCommand: null,
        runCommand: null
    });
    const [loading, setLoading] = useState<boolean>();
    const [pageLoading, setPageLoading] = useState<boolean>();
    const [error, setError] = useState<string | null>(null);
    const [errorField, setErrorField] = useState<string | null>(null);
    const [success, setSuccess] = useState<string | null>(null);
    const [problems, setProblems] = useState<Problem[]>([]);
    const [languages, setLanguages] = useState<Language[]>([]);

    const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        setError(null);
        setErrorField(null);
        const {name, value} = e.target;

        setProblemTemplateAddForm((prev) => ({...prev, [name]: value}));
    }

    const validate = () => {
        if (!problemTemplateAddForm.problemId) {
            setError("Problem is required")
            setErrorField("problemId");
            return false;
        }
        if (!problemTemplateAddForm.langId) {
            setError("Language is required")
            setErrorField("langId");
            return false;
        }
        if (!problemTemplateAddForm.methodName) {
            setError("Method name is required")
            setErrorField("methodName");
            return false;
        }
        if (!problemTemplateAddForm.returnType) {
            setError("Return type is required")
            setErrorField("returnType");
            return false;
        }
        if (problemTemplateAddForm.parameters.length <= 0) {
            setError("Parameters is required");
            setErrorField("parameters");
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

            const payload: ProblemTemplateAddRequest = {
                problemId: problemTemplateAddForm.problemId,
                langId: problemTemplateAddForm.langId,
                methodName: problemTemplateAddForm.methodName,
                returnType: problemTemplateAddForm.returnType,
                parameters: problemTemplateAddForm.parameters.map(v => `${v.type} ${v.name}`),
                compileCommand: problemTemplateAddForm.compileCommand,
                runCommand: problemTemplateAddForm.runCommand
            };

            const templateRepsonse = await problemTemplateAdd(payload);
            
            setSuccess("Problem template added with Id " + templateRepsonse.templateId);
        } catch (err: unknown) {
            const apiError = err as ApiProblemDetail;
            if (apiError.errors) {
                const entries = Object.entries(apiError.errors ?? {});
                if (entries.length > 0) {
                    const [field, messages] = entries[0];
                    setError(messages[0]);
                    setErrorField(field);
                }
            } else {
                setError("Unknown error");
            }
        } finally {
            setLoading(false);
        }
    }

    const loadProblemListLanguageList = async () => {
        try {
            setPageLoading(true);
            setError(null);

            const problemReponse = await problemList();
            const languageResponse = await languageList();

            setProblems(problemReponse);
            setLanguages(languageResponse);
        } catch (err: unknown) {
            const apiError = err as ApiProblemDetail;
            if (apiError.errors) {
                const entries = Object.entries(apiError.errors ?? {});
                if (entries.length > 0) {
                    const [field, messages] = entries[0];
                    setError(messages[0]);
                }
            }
        } finally {
            setPageLoading(false);
        }
    }

    const addParameter = (variable: Variable) => {
        setError(null);
        setErrorField(null);
        setProblemTemplateAddForm(prev => ({
            ...prev,
            parameters: [...prev.parameters, variable]
        }));
    };

    const removeParameter = (index: number) => {
        setError(null);
        setErrorField(null);
        setProblemTemplateAddForm(prev => ({
            ...prev,
            parameters: prev.parameters.filter((_, i) => i !== index)
        }));
    };

    const selectedProblem = useMemo(() => {
        if (!problemTemplateAddForm.problemId) return undefined;
        return problems.find(
            p => p.problemId === problemTemplateAddForm.problemId
        );
    }, [problemTemplateAddForm.problemId, problems]);

    const selectedLanguage = useMemo(() => {
        if (!problemTemplateAddForm.langId) return undefined;
        return languages.find(
            l => l.langId === problemTemplateAddForm.langId
        );
    }, [problemTemplateAddForm.langId, languages]);

    useEffect(() => {
        loadProblemListLanguageList();
    }, []);

    return {
        problemTemplateAddForm,
        loading,
        problems,
        languages,
        selectedProblem,
        selectedLanguage,
        error,
        errorField,
        success,
        pageLoading,
        handleChange,
        handleSubmit,
        addParameter,
        removeParameter
    };
}