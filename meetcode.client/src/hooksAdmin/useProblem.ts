import { useEffect, useState } from "react";
import { Problem } from "../types/problem";
import { problemList } from "../api/problem";

export default function useProblem() {
    const [problems, setProblems] = useState<Array<Problem>>([]);

    useEffect(() => {
        listProblems();
    }, []);

    const listProblems = async () => {
        const problemResponse = await problemList();
        setProblems(problemResponse.problemList);
    }

    return {problems};
}