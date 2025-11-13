import ReactMarkdown from "react-markdown";
import remarkGfm from "remark-gfm";

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
    tagList: Array<Tag>;
    testCaseList: Array<TestCase>;
}

interface Tag {
    tagId: string;
    name: string;
}

interface TestCase {
    testId: string;
    visibility: string;
    inputText: string;
    outputText: string;
    weight: number;
    problemId: string;
}

interface ProblemQuestionPageProps {
    problemDetail: Problem;
}

export default function ProblemQuestionPage({ problemDetail }: ProblemQuestionPageProps) {

    return (
        <>
            <style>
                        {`
                        .prose code
                        {
                            background-color: #1f2937;
                            border: 1px solid #4b5563;
                            border-width: 1px;
                            padding-inline: 2px;
                            border-radius: 2px;
                        }
                        `}
            </style>
            <div>
                <div className="prose p-5">  
                    <h1 className="font-black capitalize text-xl">
                        {problemDetail.title}
                    </h1>

                    <span className="px-3 py-1 border-gray-600 rounded-md bg-gray-800 inline-block my-5 capitalize">
                        <p className={`${problemDetail.difficulty === "easy" ? "text-green-500" : problemDetail.difficulty === "medium" ? "text-amber-300" : "text-red-500"}
                        text-sm font-medium`}>
                            {problemDetail.difficulty}
                        </p>
                    </span>

                    <ReactMarkdown remarkPlugins={[remarkGfm]}>
                        {problemDetail.statementMd}
                    </ReactMarkdown>
                </div>
            </div>
        </>
    )
}