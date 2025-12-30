import { useEffect, useState } from "react";
import { problemList as fetchProblemList } from "../api/user/problem";
import { useNavigate } from "react-router-dom";
import { Problem } from "../types/user/problem";
import { ApiProblemDetail } from "../types/system/apiProblemDetail";
import { ProblemListUserReponse } from "../types/response/problemResponses";

export default function useProblemList() {
    const [problemList, setProblemList] = useState<Problem[]>([]);
    const [problemSearchBox, setProblemSearchBox] = useState<string>("");
    const [loading, setLoading] = useState<boolean>(false);
    const [error, setError] = useState<string | null>(null);
    const [pageNumber, setPageNumber] = useState<number>(1);
    const [pageSize] = useState<number>(20);
    const [totalCount, setTotalCount] = useState<number>();

    const navigate = useNavigate();

    const handleSearch = async (e: React.ChangeEvent<HTMLInputElement>) => {
        try {
            const value = e.target.value;
            setProblemSearchBox(value);
            let response;
            if (value) {
                response = await fetchProblemSearch(value);
            } else {
                response = await fetchProblemList();
            }
            setProblemList(response.problemList);
        } catch (err: any) {
            setError(err.message || "Failed to fetch problem list");
        } finally {
            setLoading(false);
        }
        
    }

    const handleProblemList = async () => {
        try {
            setLoading(true);
            var response: ProblemListUserReponse = await fetchProblemList(pageNumber, pageSize);
            setTotalCount(response.totalCount);
            setProblemList(response.problemList);
        } catch (err: any) {
            const apiError = err as ApiProblemDetail
            if (apiError.errors) {
                const entries = Object.entries(apiError.errors ?? {});
                if (entries.length > 0) {
                    const [field, messages] = entries[0];
                    setError(messages[0]);
                }
            } else {
                setError("Failed to fetch problem list");
            }
        } finally {
            setLoading(false);
        }
    }

    useEffect(() => {
        handleProblemList();
    }, []);

    return {problemList, 
        problemSearchBox, 
        loading, 
        error, 
        handleProblemList, 
        handleSearch,
        totalCount
    };
}