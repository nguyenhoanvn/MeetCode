import useTestCaseList from "../hooks/useTestCaseList";
import { TestCase } from "../types/testCase";

interface TestCaseListPageProps{
    testCaseList: Array<TestCase>;
}

export default function TestCaseListPage(props: TestCaseListPageProps) {
    const {testCases, updateTestCase, removeTestCase, getParsedTestCases} = useTestCaseList(props.testCaseList);

    return (
        <div>
            {testCases.map(item => (
                <div key={item.testId}>Input: {item.inputJson} Output: {item.outputJson}</div>
            ))}
        </div>
    )
}