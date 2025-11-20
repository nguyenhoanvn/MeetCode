import useTabs from "../hooks/useTabs";
import useTestCaseList from "../hooks/useTestCaseList";
import { TestCase } from "../types/testCase";

interface TestCaseListPageProps{
    testCaseList: Array<TestCase>;
}

export default function TestCaseListPage(props: TestCaseListPageProps) {
    const { testCases, updateTestCase, removeTestCase, parsedTestCase, selectedTab, setSelectedTab } = useTestCaseList(props.testCaseList);

    const selectedCase = parsedTestCase[selectedTab];
    const parsedInput = selectedCase?.input;

    return (
        <>
            <div className="px-5 py-1 flex flex-row gap-3">
                {testCases.map((item, index) => (
                    <div key={item.testId} className="flex flex-col cursor-pointer relative group"
                    onClick={() => setSelectedTab(index)}>
                        <div className="absolute -right-2 -top-1 bg-gray-600 rounded-full h-4 w-4 items-center justify-center hover:bg-gray-400 hidden group-hover:flex"
                            onClick={(e) => {
                                removeTestCase(item.testId);
                            }}>
                            <span className="material-symbols-outlined !text-xs">
                                close
                            </span>
                        </div>
                        <p className={`px-3 py-1 h-full w-fit rounded-lg 
                        ${selectedTab === index ? "bg-gray-600" : "hover:bg-gray-600"}`}>
                            Test {index + 1}
                        </p>
                    </div>
                ))}
            </div>
            <div className="px-5 py-5">
                {parsedInput && (
                    <div className="flex flex-col gap-5">
                        {Object.entries(parsedInput).map(([key, value]) => (
                            <div className="flex flex-col gap-2" key={key}>
                                <p className="text-sm text-gray-400">{key} =</p>
                                <div className="px-5 py-3 bg-gray-700 text-sm rounded-lg">{value}</div>
                            </div>
                        ))}
                    </div>
                )}
            </div>
        </>
    )
}