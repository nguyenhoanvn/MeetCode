import { Splitter, SplitterPanel } from "primereact/splitter";
import useCodeEditor from "../hooks/useCodeEditor"
import { Editor } from "@monaco-editor/react";
import useTabs from "../hooks/useTabs";
import TestCaseListPage from "./TestCaseListPage";
import { TestCase } from "../types/testCase";
import useRunCode from "../hooks/useRunCode";
import { useEffect, useState } from "react";
import { Problem } from "../types/problem";
import TestResultsPage from "./TestResultsPage";

interface CodeEditorPageProps {
    problem?: Problem
}

export default function CodeEditorPage(props: CodeEditorPageProps) {
    const { selectedLanguage, code, languageDropdown, handleLanguageChange, handleDropdownClick } = useCodeEditor(props.problem);
    const { runCodeRequest, handleChangeCode, handleChangeTestCase, submitJob, loading, jobId, results } = useRunCode(selectedLanguage, props.problem?.problemId ?? "", code, props.problem?.testCaseList.map(tc => tc.testId) ?? []);
    const { selectedTab, handleSelectTab } = useTabs();


    const tabs = [
        { label: "Test Cases", icon: "task_alt", color: "text-green-400" },
        { label: "Test Results", icon: "check", color: "text-green-600" },
    ];

    return(
        <>
            <style>
                    {`
                        .editor-splitter .p-splitter-gutter {
                            width: 100% !important;
                            border-top: 1px solid white;
                            border-bottom: 1px solid white;
                            height: 10px !important;
                            background-color: transparent;
                            transition: box-shadow 0.2s ease, background-color 0.2s ease;
                        }

                        .editor-splitter .p-splitter-gutter:hover {
                            background-color: var(--color-gray-600);
                        }
                    `}   
                </style>
            <div className="h-screen w-full flex flex-col overflow-hidden"> 
                <div className="w-full h-1/14 shrink-0 flex items-center border-gray-300 border-b relative">
                    <button type="button" onClick={handleDropdownClick}
                    className={`flex items-center relative h-full left-1/30 bg-gray-700 rounded-lg w-1/6 wrap-anywhere cursor-pointer
                    ${languageDropdown ? "rounded-b-none" : ""}`}>
                        <span className="capitalize absolute left-1/8 font-black">{selectedLanguage}</span>
                        <span className="material-symbols-outlined absolute right-1/20">
                            arrow_drop_down
                        </span>
                    </button>
                    <div className={`${languageDropdown ? "block" : "hidden"}
                        bg-gray-700 absolute left-1/30 top-full w-1/6 z-10`}>
                        <div onClick={(e) => {
                            e.stopPropagation();
                            handleLanguageChange("csharp")}}
                        className={`hover:bg-white hover:text-black transition duration-300 py-1
                        ${selectedLanguage === "csharp" ? "bg-white text-black" : ""}`}>
                            <p className="text-left relative left-1/8 font-black">Csharp</p>
                        </div>
                        <div onClick={(e) => {
                            e.stopPropagation();
                            handleLanguageChange("java")}}
                        className={`hover:bg-white hover:text-black transition duration-300 py-1
                        ${selectedLanguage === "java" ? "bg-white text-black" : ""}`}>
                            <p className="text-left relative left-1/8 font-black">Java</p>
                        </div> 
                    </div>
                    <button type="button" className="absolute right-1/30 cursor-pointer" onClick={submitJob}>
                        {loading ? (
                            <span className="material-symbols-outlined">
                                clock_loader_10
                            </span>
                        ) : 
                        (
                            <span className="material-symbols-outlined">
                                play_arrow
                            </span>
                        )}
                    </button>
                </div>
                <div className="flex-1 min-h-0 relative">
                    <Splitter className="h-full editor-splitter" layout="vertical">
                        <SplitterPanel size={60}>
                            <Editor
                                height="95%"
                                language={selectedLanguage}
                                value={code}
                                theme="vs-dark"
                                onChange={handleChangeCode}
                                options={{
                                    quickSuggestions: false,
                                    suggestOnTriggerCharacters: false,
                                    wordBasedSuggestions: "off",
                                    parameterHints: { enabled: false },
                                    tabCompletion: "off",
                                    renderControlCharacters: false,
                                    renderWhitespace: "none",
                                    lineNumbers: "on",
                                    contextmenu: false,
                                    minimap: { enabled: false },
                                    fontSize: 16,
                                    padding: { top: 10 },
                                }}
                            />
                        </SplitterPanel>

                        <SplitterPanel size={40} className="min-h-10 overflow-hidden">
                            <div className="w-full h-full flex flex-col min-h-0">
                                <div className="h-10 bg-gray-800 flex flex-row items-center px-1">
                                    {tabs.map((tab, index) => (
                                        <div
                                            key={tab.label}
                                            onClick={() => handleSelectTab(index)}
                                            className="h-3/4 w-1/10 flex items-center justify-center gap-1 rounded-lg 
                                                cursor-pointer hover:bg-gray-600"
                                        >
                                            <span className={`material-symbols-outlined !text-md ${tab.color}`}>
                                                {tab.icon}
                                            </span>
                                            <span className="text-xs leading-none">{tab.label}</span>
                                        </div>
                                    ))}
                                </div>
                                <div className="flex-1 min-h-0 overflow-hidden">
                                    {selectedTab === 0 && <TestCaseListPage testCaseList={props.problem?.testCaseList ?? []} onChange={handleChangeTestCase}/>}
                                    {selectedTab === 1 && <TestResultsPage jobId={jobId} results={results}/>}
                                </div>
                            </div>
                        </SplitterPanel>
                    </Splitter>
                </div>
            </div>
        </>
    )
}