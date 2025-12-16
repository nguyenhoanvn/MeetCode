import { useEffect } from "react";
import useTestCaseList from "../hooks/useTestCaseList";
import { TestResult } from "../types/testResult";
import useTestResult from "../hooks/useTestResult";

interface TestResultsPageProps {
    jobId: string | null;
    results: TestResult[];
    onChange?: (updatedList: TestResult[]) => void;
}

export default function TestResultsPage(props: TestResultsPageProps) {

    const { results, parsedTestCase, selectedTab, setSelectedTab } = useTestResult(props.results);
    const selectedCase = parsedTestCase[selectedTab];
    const parsedInput = selectedCase?.testCase?.input ?? null;

    useEffect(() => {
        props.onChange?.(results);
    }, [results]);

    if (!results || results.length === 0) {
        return (
            <div className="h-full min-h-0 overflow-auto px-5 py-3 flex items-center justify-center">
                <p className="text-gray-400">No test results yet</p>
            </div>
        );
    }

    return (
        <div className="h-full px-5 py-5 flex flex-col gap-5 overflow-auto min-h-0">
            
                    <div className="flex flex-row items-center gap-5">
                        <div>
                            {selectedCase.isSuccessful ? 
                            (<p className="text-xl text-green-400 font-light">Accepted</p>
                            ) : 
                            (<p className="text-xl text-red-500 font-light">Rejected</p>
                            )}
                            <p className="text-2xl"></p>
                        </div>
                        <p className="text-sm text-gray-400">Runtime: {selectedCase.execTimeMs} ms</p>
                    </div>
                    <div className="flex flex-row gap-3">
                        {props.results.map((item, index) => (
                            <div key={item.testCase.testId} className="flex flex-col cursor-pointer relative group"
                            onClick={() => setSelectedTab(index)}>
                                {selectedCase.isSuccessful ?
                                    (<p className={`px-3 py-1 h-full w-fit rounded-lg border-green-400 flex items-center gap-1
                                    ${selectedTab === index ? "bg-gray-600" : "hover:bg-gray-600"}`}>
                                        <span className="material-symbols-outlined text-green-500 text-sm!">
                                            check_circle
                                        </span> Test {index + 1}
                                    </p>) :
                                    (<p className={`px-3 py-1 h-full w-fit rounded-lg border-red-500 flex items-center text-sm gap-1
                                    ${selectedTab === index ? "bg-gray-600" : "hover:bg-gray-600"}`}>
                                        <span className="material-symbols-outlined text-red-500 text-sm!">
                                            cancel
                                        </span>Case {index + 1}
                                    </p>)}
                            </div>
                        ))}
                    </div>
                    <div>
                        {parsedInput && (
                            <div className="flex flex-col gap-3">
                                <div className="flex flex-col gap-1">
                                    <p className="text-sm text-gray-400">Input</p>
                                    <div className="flex flex-col gap-3">
                                        {Object.entries(parsedInput).map(([key, value]) => (
                                            <div className="flex flex-col px-3 py-3 bg-gray-700 text-sm rounded-lg gap-1">
                                                <p className="text-sm text-gray-400">{key} =</p>
                                                <p className="text-sm text-white">{value}</p>
                                            </div>
                                        ))}
                                    </div>    
                                </div>
                                <div className="flex flex-col gap-1">
                                    <p className="text-sm text-gray-400">Output</p>
                                    <div className="flex flex-col px-3 py-3 bg-gray-700 text-sm rounded-lg gap-1">
                                        <p className={`text-sm ${selectedCase.isSuccessful ? "text-green-400" : "text-red-500"}`}>{selectedCase.result}</p>
                                    </div>
                                </div>
                                <div className="flex flex-col gap-1">
                                    <p className="text-sm text-gray-400">Expected</p>
                                    <div className="flex flex-col px-3 py-3 bg-gray-700 text-sm rounded-lg gap-1">
                                        <p className={"text-sm text-green-400"}>{selectedCase.testCase.output}</p>
                                    </div>
                                </div>
                            </div>
                        )}
                    </div>
            
        </div>
    );
}