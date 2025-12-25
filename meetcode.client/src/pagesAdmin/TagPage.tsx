import { Link } from "react-router-dom";
import LoadingOverlay from "../components/LoadingOverlay";
import useTagList from "../hooksAdmin/useTagList";
import TablePage from "./TablePage";

export default function TagPage() {
    const {tags, loading, error} = useTagList();

    return (
        <>
            {loading && (
                <LoadingOverlay message="Loading tags..." />
            )}
            <TablePage
                title="Tags"
                addAction={["Add Tag", "/admin/tags/add"]}
                error={error}
                columns={[
                    { header: "Name" },
                    { header: "Problem Linked" },
                    { header: "Detail" }
                ]}
                data={tags}
                renderRow={(tag) => (
                    <tr key={tag.tagId} className="border-y border-gray-500">
                        <td className="px-4 py-2 border-r border-gray-500">{tag.name}</td>
                        <td className="px-4 py-2 border-r border-gray-500">{tag.problems.length} problems</td>                      
                        <td className="px-4 py-2"><span className="text-blue-500 font-black hover:text-blue-300 transition duration-100"><Link to={`${tag.tagId}`}>View</Link></span></td>
                    </tr>
                )}
            />
        </>
    )
}