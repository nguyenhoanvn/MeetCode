import { useParams } from "react-router-dom";
import useProblemDetail from "../hooks/useProblemDetail";
import NavigationBar from "../components/NavigationBar";
import { Splitter, SplitterPanel } from 'primereact/splitter';
import ProblemTabs from "../components/ProblemTabs";
        

export default function ProblemDetailPage() {
    const { slug } = useParams<{ slug: string }>();
    const { problem, initLoading, initError } = useProblemDetail(slug!);

    return(
        <>
            <style>
                {`
                    .p-splitter .p-splitter-gutter {
                        width: 8px !important;
                        background-color: transparent;
                        transition: box-shadow 0.2s ease, background-color 0.2s ease;
                    }

                    .p-splitter .p-splitter-gutter:hover {
                        background-color: var(--color-gray-600);
                    }
                `}
                
            </style>
            <div className="w-screen h-screen flex flex-col">
                <NavigationBar/>
                <div className="flex-1 p-3">
                    <Splitter className="h-full">
                        <SplitterPanel size={30} 
                        className="flex align-items-center justify-content-center
                        border border-amber-50 rounded-lg overflow-hidden flex-col">
                            <ProblemTabs
                            tabs={["Question", "Solution", "Submission", "Note"]}
                            initialTab={0}
                            onTabChange={(index) => {}}
                            />
                            <div className="flex-1 w-full p-2">
                                <p>Currently chosen</p>
                            </div>
                        </SplitterPanel>
                        <SplitterPanel size={70} className="flex align-items-center justify-content-center
                        border border-amber-50 rounded-lg overflow-hidden">
                            Panel 2
                        </SplitterPanel>
                    </Splitter>
                </div>
            </div>
        </>
    )
}