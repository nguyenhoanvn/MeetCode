import { Link, useParams } from "react-router-dom"
import useProblemDetail from "../hooksAdmin/useProblemDetail";
import LoadingOverlay from "../components/LoadingOverlay";
import ReactMarkdown from "react-markdown";
import remarkGfm from "remark-gfm";
import CodeBlock from "../components/CodeBlock";
import { difficultyStyle } from "../types/admin/problem";
import { formatDate } from "../helpers/formatDate";

export default function ProblemDetailPage() {
    const { id } = useParams<{ id: string }>();
    const { problem, loading, error, toggleLoading, toggleStatus } = useProblemDetail(id!);

    return (
        <>
            {loading && (
                <LoadingOverlay message="Loading problem..." />
            )}
            <div className="h-full w-full flex flex-col p-5 pb-10 gap-10">
                <div className="flex items-center">
                    <div className="">
                        <p className="text-lg uppercase text-gray-400 font-light">Problem Detail</p>
                        <p className="text-3xl capitalize text-white">{problem?.title}</p>
                    </div>
                    <div className="ml-auto">
                        <button
                            disabled={toggleLoading}
                            onClick={() => toggleStatus()}
                            className={`px-4 py-2 w-35 border-gray-400 border rounded-lg text-sm font-medium bg-blue-900
                        ${problem?.isActive ? "hover:bg-cyan-900" : "hover:bg-purple-950"} hover:cursor-pointer`}
                        >
                            {toggleLoading ? (
                                <div className="flex gap-3 items-center justify-center">
                                    <span className="material-symbols-outlined animate-spin">
                                        progress_activity
                                    </span>
                                    <p className="">Loading...</p>
                                </div>
                            ) : problem?.isActive ?
                                (<div className="flex gap-3 items-center justify-center">
                                    <img className="h-4 w-4 animate-pulse"
                                        src="/images/green-dot.png" />
                                    <p className="text-green-400 animate">Active</p>
                                </div>) :
                                (<div className="flex gap-3 items-center justify-center">
                                    <img className="h-4 w-4 animate-pulse"
                                        src="/images/red-dot.png" />
                                    <p className="text-red-500">Disabled</p>
                                </div>)}


                        </button>
                    </div>
                </div>
                {error ? (
                    <p className="text-red-500"><span className="font-bold">Error: </span>{error}</p>
                ) : (<></>)}

                <div className="grid grid-cols-3 gap-10">
                    <div className="flex flex-col gap-3 col-span-3">
                        <p className="font-medium text-lg">ID</p>
                        <p className="font-light border-gray-500 border p-3 rounded-lg max-w-fit">{problem?.problemId}</p>
                    </div>
                    <div className="flex flex-col gap-3">
                        <p className="font-medium text-lg">Title</p>
                        <p className="font-light border-gray-500 border p-3 rounded-lg max-w-fit">{problem?.title}</p>
                    </div>
                    <div className="flex flex-col gap-3">
                        <p className="font-medium text-lg">Slug</p>
                        <p className="font-light border-gray-500 border p-3 rounded-lg max-w-fit">{problem?.slug}</p>
                    </div>
                    <div className="flex flex-col gap-3">
                        <p className="font-medium text-lg">Difficulty</p>
                        <p className={`border-gray-500 border p-3 rounded-lg max-w-fit capitalize font-medium ${difficultyStyle[problem?.difficulty!]}`}>{problem?.difficulty}</p>
                    </div>
                    <div className="flex flex-col gap-3 col-span-3">
                        <p className="font-medium text-lg">Statement Markdown</p>
                        <div className="border-gray-500 border p-3 rounded-lg min-w-150 max-w-200 w-fit">
                            <ReactMarkdown
                                remarkPlugins={[remarkGfm]}
                                components={{
                                    code({ children }) {
                                        if (!String(children).includes("\n")) {
                                            return <code className="bg-slate-800 border border-gray-600 p-1 rounded-sm">{children}</code>;
                                        }
                                        return (
                                            <CodeBlock>
                                                {String(children)}
                                            </CodeBlock>
                                        );
                                    },
                                }}
                            >
                                {problem?.statementMd}
                            </ReactMarkdown>
                        </div>
                    </div>
                    <div className="flex flex-col gap-3">
                        <p className="font-medium text-lg">Time Limit</p>
                        <div className="flex items-center gap-2">
                            <p className="font-light border-gray-500 border p-3 rounded-lg max-w-fit">{problem?.timeLimitMs}</p>
                            <p>ms</p>
                        </div>
                    </div>
                    <div className="flex flex-col gap-3 col-span-2">
                        <p className="font-medium text-lg">Memory Limit</p>
                        <div className="flex items-center gap-2">
                            <p className="font-light border-gray-500 border p-3 rounded-lg max-w-fit">{problem?.memoryLimitMb}</p>
                            <p>Mb</p>
                        </div>
                    </div>
                    <div className="flex flex-col gap-3">
                        <p className="font-medium text-lg">Created By</p>
                        <p className="font-light border-gray-500 border p-3 rounded-lg max-w-fit">{problem?.createdBy}</p>
                    </div>
                    <div className="flex flex-col gap-3">
                        <p className="font-medium text-lg">Created At</p>
                        <p className="font-light border-gray-500 border p-3 rounded-lg max-w-fit">{formatDate(problem?.createdAt)}</p>
                    </div>
                    <div className="flex flex-col gap-3">
                        <p className="font-medium text-lg">Updated At</p>
                        <p className="font-light border-gray-500 border p-3 rounded-lg max-w-fit">{problem?.updatedAt ? formatDate(problem?.updatedAt) : "unknown"}</p>
                    </div>
                    <div className="flex flex-col gap-3">
                        <p className="font-medium text-lg">Accepted Submissions</p>
                        <p className="font-light border-gray-500 border p-3 rounded-lg max-w-fit">{problem?.scoreAcceptedCount} {problem?.scoreAcceptedCount === 1 ? "submission" : "submissions"}</p>
                    </div>
                    <div className="flex flex-col gap-3">
                        <p className="font-medium text-lg">Submissions</p>
                        <Link className="w-fit" to={`/admin/submissions/${problem?.problemId}`}><p className="font-light bg-blue-900 hover:bg-blue-950 border-gray-500 border p-3 rounded-lg max-w-fit">{problem?.submissions.length} {problem?.submissions.length === 1 ? "submission" : "submissions"}</p></Link>
                    </div>
                    <div className="flex flex-col gap-3">
                        <p className="font-medium text-lg">Acceptance Rate</p>
                        <p className="font-light border-gray-500 border p-3 rounded-lg max-w-fit">{problem?.acceptanceRate ?? "NaN"}</p>
                    </div>
                    <div className="flex flex-col gap-3">
                        <p className="font-medium text-lg">Test Cases</p>
                        <Link className="w-fit" to={`/admin/test-cases/${problem?.problemId}`}><p className="font-light bg-blue-900 hover:bg-blue-950 border-gray-500 border p-3 rounded-lg max-w-fit">{problem?.testCases.length} {problem?.testCases.length === 1 ? "test case" : "test cases"}</p></Link>
                    </div>
                    <div className="flex flex-col gap-3">
                        <p className="font-medium text-lg">Tags</p>
                        <Link className="w-fit" to={`/admin/tags/${problem?.problemId}`}><p className="font-light bg-blue-900 hover:bg-blue-950 border-gray-500 border p-3 rounded-lg max-w-fit">{problem?.tags.length} {problem?.testCases.length === 1 ? "tag" : "tags"}</p></Link>
                    </div>
                    <div className="flex flex-col gap-3">
                        <p className="font-medium text-lg">Problem Templates</p>
                        <Link className="w-fit" to={`/admin/test-cases/${problem?.problemId}`}><p className="font-light bg-blue-900 hover:bg-blue-950 border-gray-500 border p-3 rounded-lg max-w-fit">{problem?.problemTemplates.length} {problem?.testCases.length === 1 ? "template" : "templates"}</p></Link>
                    </div>
                </div>
            </div>
        </>
    )
}