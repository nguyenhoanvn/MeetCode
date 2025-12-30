import { useEffect, useState } from "react";
import { SubmissionAllResponse } from "../types/response/submissionResponses";
import { Submission } from "../types/user/submission";
import { submissionGet } from "../api/user/user";

interface UseProblemSubmissionListPageProps {
    userId: string;
    problemId: string;
}

export default function useProblemSubmissionListPage({ userId, problemId }: UseProblemSubmissionListPageProps) {
    const [submissions, setSubmissions] = useState<Submission[]>([]);
    const [loading, setLoading] = useState<boolean>(false);

    useEffect(() => {
        fetchSubmissions();
    }, [userId, problemId]);

    const fetchSubmissions = async () => {
        try {
            setLoading(true);

            const response = await submissionGet({ userId, problemId });
            console.log(response);

            setSubmissions(response.submissions || []);
        } catch (err: any) {
        } finally {
            setLoading(false);
        }
    };

    return {
        submissions,
        loading
    }
}