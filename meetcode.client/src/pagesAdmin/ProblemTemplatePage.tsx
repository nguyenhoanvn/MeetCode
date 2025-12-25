import { Link } from "react-router-dom";
import useProblemTemplateList from "../hooksAdmin/useProblemTemplateList";
import TablePage from "./TablePage";
import LoadingOverlay from "../components/LoadingOverlay";

export default function ProblemTemplatePage() {
    const { problemTemplates, loading, error } = useProblemTemplateList();

    return (
        <>
            {loading && (
                <LoadingOverlay message="Loading templates..." />
            )}
            <TablePage
                title="Problem Templates"
                error={error}
                addAction={["Add Template", "/admin/problem-templates/add"]}
                columns={[
                    { header: "Problem" },
                    { header: "Language" },
                    { header: "Status" },
                    { header: "Detail" }
                ]}
                data={problemTemplates}
                renderRow={(problemTemplate) => (
                    <tr key={problemTemplate.templateId} className="border-y border-gray-500">
                        <td className="px-4 py-2 border-r border-gray-500">{problemTemplate.problem.title}</td>
                        <td className="px-4 py-2 border-r border-gray-500">{problemTemplate.language.name}</td>
                        <td className="px-4 py-2 border-r border-gray-500">{problemTemplate.isEnabled ? <p className="text-green-400">Active</p> : <p className="text-red-500">Disabled</p>}</td>
                        <td className="px-4 py-2"><span className="text-blue-500 font-black hover:text-blue-300 transition duration-100"><Link to={`${problemTemplate.templateId}`}>View</Link></span></td>
                    </tr>
                )}
            />
        </>
    )
}