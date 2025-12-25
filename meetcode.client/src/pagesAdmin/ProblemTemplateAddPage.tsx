import { Select, SelectContent, SelectIcon, SelectItem, SelectTrigger, SelectValue } from "@radix-ui/react-select";
import LoadingOverlay from "../components/LoadingOverlay";
import useProblemTemplateAdd from "../hooksAdmin/useProblemTemplateAdd";
import { useState } from "react";
import { Variable } from "../types/system/variable";

export default function ProblemTemplateAddPage() {
    const { problemTemplateAddForm, error, errorField, loading, success, pageLoading, problems, languages, selectedProblem, selectedLanguage,
        handleChange, handleSubmit, addParameter, removeParameter } = useProblemTemplateAdd();

    const inputClass = (fieldName: string) =>
        `font-light border-gray-400 border p-3 rounded-lg max-w-fit duration-300 focus:outline-none
     ${errorField === fieldName
            ? "border-red-500 focus:border-red-500"
            : "focus:border-[#1e3a8a]"
        }`;

    const [addingParam, setAddingParam] = useState(false);
    const [draftParam, setDraftParam] = useState<Variable>({
        type: "",
        name: ""
    });
    const confirmAddParameter = () => {
        if (!draftParam.type.trim() || !draftParam.name.trim()) return;

        addParameter({
            type: draftParam.type.trim(),
            name: draftParam.name.trim()
        });

        setDraftParam({ type: "", name: "" });
        setAddingParam(false);
    };
    const [showAddCommand, setShowAddCommand] = useState<boolean>(false);

    return (
        <>
            {pageLoading && (
                <LoadingOverlay message="Loading problem form..." />
            )}
            <div className="h-full w-full flex flex-col p-5 pb-10 gap-10">
                {/* Header and Add button */}
                <div className="flex items-center">
                    <div className="">
                        <p className="text-lg uppercase text-gray-400 font-light">Template</p>
                        <p className="text-3xl capitalize text-white">Add New Template</p>
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
                                <p>Add template</p>
                            </div>)}
                        </button>
                    </div>
                </div>

                {/* Error message */}
                {error ? (
                    <p className="text-red-500"><span className="font-bold">Error: </span>{error}</p>
                ) : (<></>)}
                {/* Sucess message */}
                {success ? (
                    <p className="text-green-400"><span className="font-bold">Success: </span>{success}</p>
                ) : (<></>)}

                {/* Main content */}
                <div className="grid grid-cols-2 gap-10">
                    {/* Problem select input */}
                    <div className="flex flex-col gap-3">
                        <p className="font-medium text-lg">Problem</p>
                        <Select value={problemTemplateAddForm.problemId} onValueChange={(value) =>
                            handleChange({
                                target: {
                                    name: "problemId",
                                    value
                                }
                            } as React.ChangeEvent<HTMLInputElement>)
                        }>
                            <SelectTrigger className="font-light border-gray-400 border p-3 rounded-lg w-100 flex items-center">
                                <SelectValue placeholder="Select a problem">
                                    <span className="capitalize">
                                        {selectedProblem?.title}
                                    </span>
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
                            <SelectContent className="bg-gray-800 rounded-lg border border-gray-500 w-100"
                                position="popper" sideOffset={0}>
                                {problems.map((problem) => (
                                    <SelectItem
                                        key={problem.problemId}
                                        value={problem.problemId}
                                        className="p-3 hover:bg-gray-700 border-b border-gray-500 last:border-b-0"
                                    >
                                        {problem.title}
                                    </SelectItem>
                                ))}
                            </SelectContent>
                        </Select>
                    </div>
                    {/* Language select input */}
                    <div className="flex flex-col gap-3">
                        <p className="font-medium text-lg">Language</p>
                        <Select value={problemTemplateAddForm.langId} onValueChange={(value) =>
                            handleChange({
                                target: {
                                    name: "langId",
                                    value
                                }
                            } as React.ChangeEvent<HTMLInputElement>)
                        }>
                            <SelectTrigger className="font-light border-gray-400 border p-3 rounded-lg w-100 flex items-center">
                                <SelectValue placeholder="Select a language">
                                    <span className="capitalize">
                                        {selectedLanguage?.name}
                                    </span>
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
                            <SelectContent className="bg-gray-800 rounded-lg border border-gray-500 w-100"
                                position="popper" sideOffset={0}>
                                {languages.map((language) => (
                                    <SelectItem
                                        key={language.langId}
                                        value={language.langId}
                                        className="p-3 hover:bg-gray-700 capitalize border-b border-gray-500 last:border-b-0"
                                    >
                                        {language.name}
                                    </SelectItem>
                                ))}
                            </SelectContent>
                        </Select>
                    </div>

                    {/* Method name input */}
                    <div className="flex flex-col gap-3">
                        <p className="font-medium text-lg">Method Name</p>
                        <div className="flex gap-3 items-center">
                            <input type="text" autoComplete="off" autoCorrect="off" placeholder="twoSum" name="methodName" onChange={handleChange} value={problemTemplateAddForm.methodName}
                                className={`${inputClass("methodName")}`} />
                        </div>
                    </div>

                    {/* Return type input */}
                    <div className="flex flex-col gap-3">
                        <p className="font-medium text-lg">Return Type</p>
                        <div className="flex gap-3 items-center">
                            <input type="text" autoComplete="off" autoCorrect="off" placeholder="int" name="returnType" onChange={handleChange} value={problemTemplateAddForm.returnType}
                                className={`${inputClass("returnType")}`} />
                        </div>
                    </div>

                    {/* Parameter input */}
                    <div className="flex flex-col gap-3 col-span-2">
                        <p className="font-medium text-lg">Parameters</p>

                        {/* Pills */}
                        <div className="flex gap-10 items-center">
                            <div className="flex">
                                <div className="flex px-2 py-3 gap-1 border rounded-lg border-gray-400 max-w-300 min-w-100 min-h-15">
                                    {problemTemplateAddForm.parameters.map((param, i) => (
                                        <div
                                            key={i}
                                            className="flex items-center gap-1 bg-gray-700 px-3 py-1 rounded-lg text-sm"
                                        >
                                            <span className="text-blue-400">{param.type}</span><span>{param.name}</span>
                                            <button
                                                type="button"
                                                onClick={() => removeParameter(i)}
                                                className="hover:text-blue-500 transition duration-300 ml-auto"
                                            >
                                                <span className="material-symbols-outlined text-lg!">
                                                    close_small
                                                </span>
                                            </button>
                                        </div>
                                    ))}
                                </div>
                            </div>

                            {/* Builder */}
                            {addingParam ? (
                                <div className="flex gap-2 items-center">
                                    <input
                                        placeholder="type"
                                        autoComplete="off" autoCorrect="off"
                                        value={draftParam.type}
                                        onChange={e =>
                                            setDraftParam(p => ({ ...p, type: e.target.value }))
                                        }
                                        className="border border-gray-400 rounded-lg px-3 py-2 w-32"
                                    />
                                    <input
                                        placeholder="name"
                                        autoComplete="off" autoCorrect="off"
                                        value={draftParam.name}
                                        onChange={e =>
                                            setDraftParam(p => ({ ...p, name: e.target.value }))
                                        }
                                        className="border border-gray-400 rounded-lg px-3 py-2 w-40"
                                    />
                                    <button
                                        type="button"
                                        onClick={confirmAddParameter}
                                        className="bg-blue-600 border-gray-600 hover:bg-blue-500 border px-3 py-2 rounded-lg"
                                    >
                                        Confirm
                                    </button>
                                    <button
                                        type="button"
                                        onClick={() => setAddingParam(false)}
                                        className="border px-3 py-2 rounded-lg border-gray-600 hover:bg-gray-700 text-gray-400"
                                    >
                                        Cancel
                                    </button>
                                </div>
                            ) : (
                                <button
                                    type="button"
                                    onClick={() => setAddingParam(true)}
                                    className="w-fit h-10 bg-gray-700 hover:bg-gray-600 transition duration-300 px-3 py-2 rounded-lg text-sm"
                                >
                                    + Add variable
                                </button>
                            )}
                        </div>
                    </div>

                    <div className="flex flex-col gap-10">
                        <div className="flex gap-3 items-center">
                            <p className="font-medium text-lg">Show advanced options</p>
                            <button
                                className="flex w-7 h-7 rounded-lg border items-center justify-center"
                                onClick={() => setShowAddCommand(v => !v)}
                            >
                                <span className="material-symbols-outlined text-lg!">
                                    {showAddCommand ? "keyboard_arrow_down" : "chevron_right"}
                                </span>
                            </button>
                        </div>

                        {/* Animated container */}
                        <div
                            className={`
                                grid grid-cols-2 gap-10 overflow-hidden transition duration-300 ease-out
                                ${showAddCommand
                                ? "max-h-96 opacity-100 translate-y-0"
                                : "max-h-0 opacity-0 -translate-y-3"}
                            `}
                        >
                            {/* Compile command */}
                            <div className="flex flex-col gap-3">
                                <p className="font-medium text-lg">Compile Command</p>
                                <input
                                    type="text"
                                    autoComplete="off" autoCorrect="off"
                                    placeholder="dotnet build"
                                    name="compileCommand"
                                    onChange={handleChange}
                                    value={problemTemplateAddForm.compileCommand ?? ""}
                                    className={inputClass("compileCommand")}
                                />
                            </div>

                            {/* Run command */}
                            <div className="flex flex-col gap-3">
                                <p className="font-medium text-lg">Run Command</p>
                                <input
                                    type="text"
                                    autoComplete="off" autoCorrect="off"
                                    placeholder="dotnet run"
                                    name="runCommand"
                                    onChange={handleChange}
                                    value={problemTemplateAddForm.runCommand ?? ""}
                                    className={inputClass("runCommand")}
                                />
                            </div>
                        </div>
                    </div>

                </div>
            </div>
        </>
    )
}