import useProblemSubmissionListPage from "../hooks/useProblemSubmissionListPage";

interface ProblemSubmissionListPageProps {
    problemId: string;
    userId: string;
}

export default function ProblemSubmissionListPage({ problemId, userId }: ProblemSubmissionListPageProps) {

    const { submissions, loading } = useProblemSubmissionListPage({
        userId: userId || "",
        problemId: problemId || "",
    });

    const formatVerdict = (verdict: string) =>
        verdict
            .replace(/_/g, " ")
            .toLowerCase()
            .replace(/\b\w/g, c => c.toUpperCase());

    const verdictClass = (verdict: string) =>
        verdict.toLowerCase() === "accepted"
            ? "text-green-400"
            : "text-red-400";


    if (loading) return <div>Loading...</div>;

    return (
        <div className="overflow-x-auto">
            <table className="w-full border-collapse text-sm">
                <thead>
                    <tr className="border-b border-amber-50 text-gray-300">
                        <th className="text-left p-3">Verdict</th>
                        <th className="text-left p-3">Language</th>
                        <th className="text-left p-3">Exec Time (ms)</th>
                    </tr>
                </thead>

                <tbody>
                    {submissions.map((s) => (
                        <tr
                            key={s.submissionId}
                            className="border-b border-amber-50 hover:bg-gray-800 transition"
                        >
                            <td className={`p-3 font-medium ${verdictClass(s.verdict)}`}>
                                {formatVerdict(s.verdict)}
                            </td>
                            <td className="p-3">{s.language.name}</td>
                            <td className="p-3">{s.execTimeMs}</td>
                        </tr>
                    ))}
                </tbody>
            </table>
        </div>
    );

}