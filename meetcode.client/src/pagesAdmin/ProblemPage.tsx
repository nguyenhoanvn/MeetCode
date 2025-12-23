import { Link } from "react-router-dom";
import useProblemList from "../hooksAdmin/useProblemList";
import TablePage from "./TablePage";
import LoadingOverlay from "../components/LoadingOverlay";

export default function ProblemPage() {
    const { problems, loading, error } = useProblemList();
    return (
        <>
            {loading && (
                <LoadingOverlay message="Loading problems..." />
            )}
            <TablePage
                title="Problems"
                addAction="Add Problem"
                error={error}
                columns={[
                    { header: "Title" },
                    { header: "Difficulty" },
                    { header: "Created By" },
                    { header: "Updated At" },
                    { header: "Acceptance Rate" },
                    { header: "Status" },
                    { header: "Detail" }
                ]}
                data={problems}
                renderRow={(problem) => (
                    <tr key={problem.problemId} className="border-y border-gray-500">
                        <td className="px-4 py-2 border-r border-gray-500">{problem.title}</td>
                        <td className="px-4 py-2 border-r border-gray-500">{problem.difficulty}</td>
                        <td className="px-4 py-2 border-r border-gray-500">{problem.createdBy ? "none" : "none"}</td>
                        <td className="px-4 py-2 border-r border-gray-500">{problem.updatedAt ? "none" : "none"}</td>
                        <td className="px-4 py-2 border-r border-gray-500">{problem.acceptanceRate ? "none" : "none"}</td>
                        <td className="px-4 py-2 border-r border-gray-500">{problem.isActive ? <p className="text-green-400">Active</p> : <p className="text-red-500">Disabled</p>}</td>
                        <td className="px-4 py-2"><span className="text-blue-500 font-black hover:text-blue-300 transition duration-100"><Link to={`${problem.problemId}`}>View</Link></span></td>
                    </tr>
                )}
            />
        </>

    )
}