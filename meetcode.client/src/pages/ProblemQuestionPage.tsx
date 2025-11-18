import ReactMarkdown from "react-markdown";
import remarkGfm from "remark-gfm";
import CodeBlock from "../components/CodeBlock";
import { Problem } from "../types/problem";

interface ProblemQuestionPageProps {
    problemDetail: Problem;
}

export default function ProblemQuestionPage({ problemDetail }: ProblemQuestionPageProps) {

    const md = `
Given two numbers \`a\` and \`b\`, return their sum

**Example 1:**

\`\`\`
Input: a = 1, b = 2
Output: 3
\`\`\`
`;

    return (
        <>
            <style>
                        {`
                        .prose .code-inline
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

                    <ReactMarkdown
                        remarkPlugins={[remarkGfm]}
                        components={{
                            code({ children }) {
                                if (!String(children).includes("\n")) {
                                    return <code className="code-inline">{children}</code>;
                                }    
                                return (
                                    <CodeBlock>
                                        {String(children)}
                                    </CodeBlock>
                                );
                            },
                        }}
                    >
                        {md}
                    </ReactMarkdown>
                </div>
            </div>
        </>
    )
}