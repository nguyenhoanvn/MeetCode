import { Link, useParams } from "react-router-dom";
import useProblemTemplateDetail from "../hooksAdmin/useProblemTemplateDetail";
import { Editor } from "@monaco-editor/react";
import LoadingOverlay from "../components/LoadingOverlay";

export default function ProblemTemplateDetailPage() {
    const { id } = useParams<{ id: string }>();
    const { problemTemplate, error, loading, toggleStatus, toggleLoading } = useProblemTemplateDetail(id!);

    return (
        <>
            {loading && (
                <LoadingOverlay message="Loading template..."/>
            )}
            <div className="h-full w-full flex flex-col p-5 pb-10 gap-10">
                <div className="flex items-center">
                    <div className="">
                        <p className="text-lg uppercase text-gray-400 font-light">Problem Template Detail</p>
                        <p className="text-3xl capitalize text-white">{problemTemplate?.problem.title} - {problemTemplate?.language.name}</p>
                    </div>
                    <div className="ml-auto">
                        <button
                            disabled={toggleLoading}
                            onClick={() => toggleStatus()}
                            className={`px-4 py-2 w-35 border-gray-400 border rounded-lg text-sm font-medium bg-blue-900
                        ${problemTemplate?.isEnabled ? "hover:bg-cyan-900" : "hover:bg-purple-950"} hover:cursor-pointer`}
                        >
                            {toggleLoading ? (
                                <div className="flex gap-3 items-center justify-center">
                                    <span className="material-symbols-outlined animate-spin">
                                        progress_activity
                                    </span>
                                    <p className="">Loading...</p>
                                </div>
                            ) : problemTemplate?.isEnabled ?
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

                <div className="grid grid-cols-2 gap-10">
                    <div className="flex flex-col gap-3 col-span-2">
                        <p className="font-medium text-lg">ID</p>
                        <p className="font-light border-gray-500 border p-3 rounded-lg max-w-fit">{problemTemplate?.templateId}</p>
                    </div>
                    <div className="flex flex-col gap-3">
                        <p className="font-medium text-lg">Problem</p>
                        <Link className="w-fit" to={`/admin/problems/${problemTemplate?.problemId}`}><p className="font-light bg-blue-900 hover:bg-blue-950 border-gray-500 border p-3 rounded-lg max-w-fit">{problemTemplate?.problem.title}</p></Link>
                    </div>
                    <div className="flex flex-col gap-3">
                        <p className="font-medium text-lg">Language</p>
                        <Link className="w-fit" to={`/admin/languages/${problemTemplate?.langId}`}><p className="font-light bg-blue-900 hover:bg-blue-950 border-gray-500 border p-3 rounded-lg max-w-fit">{problemTemplate?.language.name}</p></Link>
                    </div>
                    <div className="flex flex-col gap-3">
                        <p className="font-medium text-lg">Compile Command</p>
                        <p className="font-light border-gray-500 border p-3 rounded-lg max-w-fit">{problemTemplate?.compileCommand ?? "None"}</p>
                    </div>
                    <div className="flex flex-col gap-3">
                        <p className="font-medium text-lg">Run Command</p>
                        <p className="font-light border-gray-500 border p-3 rounded-lg max-w-fit">{problemTemplate?.runCommand ?? "None"}</p>
                    </div>
                    <div className="flex flex-col gap-3 col-span-2 h-75 w-250">
                        <p className="font-medium text-lg">Starter Code</p>
                        <Editor
                            className="border-gray-500 border"
                            height="100%"
                            language={problemTemplate?.language.name}
                            value={problemTemplate?.templateCode}
                            theme="vs-dark"
                            options={{
                                readOnly: true,
                                quickSuggestions: false,
                                suggestOnTriggerCharacters: false,
                                wordBasedSuggestions: "off",
                                parameterHints: { enabled: false },
                                tabCompletion: "off",
                                renderControlCharacters: false,
                                renderWhitespace: "none",
                                lineNumbers: "off",
                                contextmenu: false,
                                minimap: { enabled: false },
                                fontSize: 16,
                                padding: { top: 10 },
                            }}
                        />
                    </div>
                    <div className="flex flex-col gap-3 col-span-2 h-100 w-250">
                        <p className="font-medium text-lg">Runner Code</p>
                        <Editor
                            className="border-gray-500 border"
                            height="100%"
                            language={problemTemplate?.language.name}
                            value={problemTemplate?.runnerCode}
                            theme="vs-dark"
                            options={{
                                readOnly: true,
                                quickSuggestions: false,
                                suggestOnTriggerCharacters: false,
                                wordBasedSuggestions: "off",
                                parameterHints: { enabled: false },
                                tabCompletion: "off",
                                renderControlCharacters: false,
                                renderWhitespace: "none",
                                lineNumbers: "off",
                                contextmenu: false,
                                minimap: { enabled: false },
                                fontSize: 16,
                                padding: { top: 10 },
                            }}
                        />
                    </div>

                </div>
            </div>
        </>
    );
}