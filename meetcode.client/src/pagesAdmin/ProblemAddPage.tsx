import { useState } from "react";
import LoadingOverlay from "../components/LoadingOverlay";
import useProblemAdd from "../hooksAdmin/useProblemAdd";
import { Select, SelectContent, SelectIcon, SelectItem, SelectTrigger, SelectValue } from "@radix-ui/react-select";
import { difficultyStyle } from "../types/admin/problem";
import Markdown from "react-markdown";
import remarkGfm from "remark-gfm";
import CodeBlock from "../components/CodeBlock";

export default function ProblemAddPage() {
    const { problemAddForm, pageLoading, tagNameQuery, handleChange,
        handleSubmit, loading, error, errorField, previewStatementMd, handleTagDelete,
        togglePreview, handleQueryChange, suggestions, handleTagAdd, success } = useProblemAdd();

    const inputClass = (fieldName: string) =>
        `font-light border-gray-500 border p-3 rounded-lg max-w-fit duration-300 focus:outline-none
     ${errorField === fieldName
            ? "border-red-500 focus:border-red-500"
            : "focus:border-[#1e3a8a]"
        }`;

    return (
        <>
            {pageLoading && (
                <LoadingOverlay message="Loading problem form..." />
            )}
            <div className="h-full w-full flex flex-col p-5 pb-10 gap-10">
                {/* Header and Add button */}
                <div className="flex items-center">
                    <div className="">
                        <p className="text-lg uppercase text-gray-400 font-light">Problem</p>
                        <p className="text-3xl capitalize text-white">Add New Problem</p>
                    </div>
                    <div className="ml-auto">
                        <button
                            disabled={loading}
                            onClick={handleSubmit}
                            className="py-2 w-35 border-gray-400 border rounded-lg text-sm font-medium bg-blue-900 hover:cursor-pointer hover:bg-blue-950"
                        >
                            {loading ? (
                                <div className="flex items-center justify-center">
                                    <span className="material-symbols-outlined animate-spin">
                                        progress_activity
                                    </span>
                                    <p className="">Loading...</p>
                                </div>
                            ) : (<div className="flex gap-3 items-center justify-center">
                                <span className="material-symbols-outlined text-sm!">
                                    add
                                </span>
                                <p>Add problem</p>
                            </div>)}
                        </button>
                    </div>
                </div>

                {/* Error message */}
                {error ? (
                    <p className="text-red-500"><span className="font-bold">Error: </span>{error}</p>
                ) : (<></>)}
                {/* Sucess message */}
                {success == null ? (
                    <p className="text-green-400"><span className="font-bold">Success: </span>{success}</p>
                ) : (<></>)}

                {/* Main content */}
                <div className="grid grid-cols-2 gap-10">
                    {/* Title input */}
                    <div className="flex flex-col gap-3">
                        <p className="font-medium text-lg">Title</p>
                        <input type="text" name="title" value={problemAddForm.title} onChange={handleChange} placeholder="solve me first"
                            className={`${inputClass("title")}`} />
                    </div>
                    {/* Difficulty select dropdown */}
                    <div className="flex flex-col gap-3">
                        <p className="font-medium text-lg">Difficulty</p>
                        <div className="flex items-center">
                            <Select value={problemAddForm.difficulty} onValueChange={(value) =>
                                handleChange({
                                    target: {
                                        name: "difficulty",
                                        value
                                    }
                                } as React.ChangeEvent<HTMLInputElement>)
                            }>
                                <SelectTrigger className="font-light border-gray-500 border p-3 rounded-lg w-35 flex items-center">
                                    <SelectValue aria-label={problemAddForm.difficulty}>
                                        <span className={`capitalize ${difficultyStyle[problemAddForm?.difficulty]}`}>{problemAddForm.difficulty}</span>
                                    </SelectValue>
                                    <SelectIcon className="ml-auto flex">
                                        <span
                                            className="material-symbols-outlined
                                                    pointer-events-none text-gray-400"
                                        >
                                            expand_more
                                        </span>
                                    </SelectIcon>
                                </SelectTrigger>
                                <SelectContent className="bg-gray-800 rounded-lg border border-gray-500 w-35"
                                    position="popper" sideOffset={0}>
                                    <SelectItem value="easy" className={`p-3 hover:bg-gray-700 border-b border-gray-500 rounded-t-lg`}>
                                        <span className={`${difficultyStyle["easy"]}`}>Easy</span>
                                    </SelectItem>
                                    <SelectItem value="normal" className={`p-3 hover:bg-gray-700 border-b border-gray-500`}>
                                        <span className={`${difficultyStyle["medium"]}`}>Medium</span>
                                    </SelectItem>
                                    <SelectItem value="hard" className={`p-3 hover:bg-gray-700 border-gray-500 rounded-b-lg`}>
                                        <span className={`${difficultyStyle["hard"]}`}>Hard</span>
                                    </SelectItem>
                                </SelectContent>
                            </Select>
                        </div>
                    </div>
                    {/* Time limit input */}
                    <div className="flex flex-col gap-3">
                        <p className="font-medium text-lg">Time Limit</p>
                        <input type="number" placeholder="10" name="timeLimitMs" onChange={handleChange} value={problemAddForm.timeLimitMs}
                            className={`${inputClass("timeLimitMs")}`} />
                    </div>
                    {/* Memory limit input */}
                    <div className="flex flex-col gap-3">
                        <p className="font-medium text-lg">Memory Limit</p>
                        <input type="number" placeholder="10" name="memoryLimitMb" onChange={handleChange} value={problemAddForm.memoryLimitMb}
                            className={`${inputClass("memoryLimitMb")}`} />
                    </div>
                    {/* Tag input */}
                    <div className="flex flex-col gap-3">
                        <p className="font-medium text-lg">Tags</p>
                        <div className="flex gap-30 items-center">

                            <input
                                type="text"
                                name="tag"
                                value={tagNameQuery}
                                onChange={handleQueryChange}
                                className={`${inputClass("tag")}`}
                                placeholder="Type a tag..."
                            />
                            <div className="h-15 w-150 border border-gray-500 rounded-lg">
                                <div className="flex items-center h-full p-2">
                                    {problemAddForm.tags.map((tag) => (
                                        <span
                                            key={tag.tagId}
                                            className="flex capitalize items-center gap-1 bg-blue-700 px-3 py-2 rounded text-sm"
                                        >
                                            {tag.name}
                                            <button
                                                type="button"
                                                className="text-gray-300 flex items-center hover:text-white"
                                                onClick={() => handleTagDelete(tag.tagId)}
                                            >
                                                <span className="material-symbols-outlined">
                                                    close_small
                                                </span>
                                            </button>
                                        </span>
                                    ))}
                                </div>
                                <ul className={`relative w-150 top-1 h-15 flex items-center transition duration-300 
                                    p-2 rounded-lg overflow-hidden border-gray-500 border ${suggestions.length > 0 ? "opacity-100" : "opacity-0"}`}>
                                    {suggestions.map((s) => (
                                        <li
                                            key={s.tagId}
                                            className="px-3 py-1 rounded-lg hover:bg-gray-700 w-fit cursor-pointer"
                                            onClick={() => { handleTagAdd(s) }}
                                        >
                                            {s.name}
                                        </li>
                                    ))}
                                </ul>
                            </div>
                        </div>
                    </div>
                    {/* Statement markdown input */}
                    <div className="flex flex-col gap-3 col-span-2">
                        <p className="font-medium text-lg">Statement Markdown</p>

                        <div className={`flex flex-col w-3/4 h-100 border ${errorField === "statementMd"
                                                ? "border-red-500 focus:border-red-500"
                                                : "focus:border-[#1e3a8a]"
                                            } rounded-lg overflow-hidden`}>
                            <div className="w-full h-15 border-b border-gray-500 flex items-center p-3">
                                <div className="flex border border-gray-500 rounded-lg items-center">
                                    <button type="button" onClick={togglePreview}
                                        className={`px-5 text-sm ${!previewStatementMd ? "font-bold" : "font-light"}`}>
                                        Edit
                                    </button>
                                    <div className="w-px h-10 bg-gray-500" />
                                    <button type="button" onClick={togglePreview}
                                        className={`px-7 text-sm ${previewStatementMd ? "font-bold" : "font-light"}`}>
                                        Preview
                                    </button>
                                </div>
                            </div>
                            <div className="p-3 h-full bg-gray-900 text-sm font-medium font-consolas">
                                {!previewStatementMd ? (
                                    <textarea
                                        name="statementMd"
                                        value={problemAddForm.statementMd}
                                        onChange={handleChange}
                                        placeholder="Enter problem markdown content here"
                                        className="w-full h-60 bg-transparent outline-none resize-none"
                                    />
                                ) : (
                                    <div className="prose prose-invert max-w-none">
                                        <Markdown
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
                                            }}>
                                            {problemAddForm.statementMd || ""}
                                        </Markdown>
                                    </div>
                                )}
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </>
    )
}