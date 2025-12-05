import { useParams } from "react-router-dom";
import useProblemDetail from "../hooks/useProblemDetail";
import NavigationBar from "../components/NavigationBar";
import { Splitter, SplitterPanel } from 'primereact/splitter';
import useTabs from "../hooks/useTabs";
import ProblemQuestionPage from "./ProblemQuestionPage";
import CodeEditorPage from "./CodeEditorPage";
        

export default function ProblemDetailPage() {
    const { slug } = useParams<{ slug: string }>();
    const { problem, initLoading, initError } = useProblemDetail(slug!);
    const { selectedTab, handleSelectTab } = useTabs();

    const tabs = ["Question", "Solution", "Submission", "Note"];

    return(
        <>
            <style>
                {`
                    .problem-splitter .p-splitter-gutter {
                        width: 8px !important;
                        height: 100%;
                        background-color: transparent;
                        transition: box-shadow 0.2s ease, background-color 0.2s ease;
                    }

                    .problem-splitter .p-splitter-gutter:hover {
                        background-color: var(--color-gray-600);
                    }
                `}
                
            </style>
            <div className="w-screen h-screen flex flex-col">
                <NavigationBar/>
                <div className="flex-1 p-3">
                    <Splitter className="h-full problem-splitter" layout="horizontal">
                        <SplitterPanel size={40} 
                        className="flex align-items-center justify-content-center
                        border border-amber-50 rounded-lg overflow-hidden flex-col">
                           <div className="w-full h-1/14 flex items-center border-gray-300 border-b">
                                <ul className="flex w-11/12 text-sm h-full">
                                    {tabs.map((tab, index) => (
                                        <li 
                                        key={tab}
                                        onClick={() => handleSelectTab(index)}

                                        className={`
                                        cursor-pointer transition flex items-center justify-center px-4
                                        ${selectedTab === index ? "text-white border-b" : "text-gray-300"}
                                        `}
                                        >
                                            {tab}
                                        </li>
                                    ))}
                                </ul>
                                
                            </div>
                            <div className="flex-1 w-full p-2">
                                <div> 
                                    {selectedTab === 0 && problem && <ProblemQuestionPage problemDetail={problem} />}
                                    {selectedTab === 1 && <p>Currently chosen Solution</p>}
                                    {selectedTab === 2 && <p>Currently chosen Submission</p>}
                                    {selectedTab === 3 && <p>Currently chosen Note</p>}
                                </div>
                            </div>
                        </SplitterPanel>
                        <SplitterPanel size={60} className="flex align-items-center justify-content-center
                        border border-amber-50 rounded-lg overflow-hidden relative h-full flex-col">
                            {initLoading ? (
                                <div>Loading...</div>
                            ) : (
                                <CodeEditorPage problemId={problem?.problemId ?? ""} testCaseList={problem?.testCaseList ?? []}/>
                            )}                 
                        </SplitterPanel>
                    </Splitter>
                </div>
            </div>
        </>
    )
}