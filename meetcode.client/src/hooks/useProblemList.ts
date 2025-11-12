import { useEffect, useState } from "react";
import { problemList as fetchProblemList } from "../api/problem";
import { problemSearch as fetchProblemSearch } from "../api/problem";
import { useNavigate } from "react-router-dom";

enum Difficulty {
    Easy = 'easy',
    Medium = 'medium',
    Hard = 'hard'
}

interface Problem {
    problemId: string;
    title: string;
    slug: string;
    statementMd: string;
    difficulty: Difficulty;
    totalSubmissionCount: number;
    scoreAcceptedCount: number;
    acceptanceRate: number;
}

export default function useProblemList() {
    const [problemList, setProblemList] = useState<Array<Problem>>([]);
    const [problemSearchBox, setProblemSearchBox] = useState<string>("");
    const [loading, setLoading] = useState<boolean>(false);
    const [error, setError] = useState<string | null>(null);

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
            var response = await fetchProblemList();
            setProblemList(response.problemList);
            console.log(response);
        } catch (err: any) {
            setError(err.message || "Failed to fetch problem list");
        } finally {
            setLoading(false);
        }
    }

    useEffect(() => {
        handleProblemList();
    }, []);

    return {problemList, problemSearchBox, loading, error, handleProblemList, handleSearch};
}