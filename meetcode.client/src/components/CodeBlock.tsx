import useCodeBlock from "../hooks/useCodeBlock";

interface CodeBlockProps {
    children: string;
}

export default function CodeBlock(input: CodeBlockProps) {
    const {copied, handleCopyToClipboard} = useCodeBlock(input.children);

    return (
        <div className="relative group my-3">
            <div className="absolute right-3 top-3 bg-gray-400 px-2 py-0.5 rounded-md
            opacity-0 group-hover:opacity-100 transition">
                <button className="cursor-pointer"
                onClick={handleCopyToClipboard}>
                    {copied ? "Copied" : "Copy"}
                </button>
            </div>
            <pre className="rounded-lg bg-gray-900 p-3 overflow-auto leading-10">
                <code className="">{input.children}</code>
            </pre>
        </div>
    )
}