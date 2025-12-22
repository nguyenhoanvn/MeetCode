import { Link } from "react-router-dom";
import useLanguageList from "../hooksAdmin/useLanguageList";
import TablePage from "./TablePage";

export default function LanguagePage() {
    const { languages } = useLanguageList();

    return (
        <TablePage
            title="Languages"
            columns={[
                { header: "Name" },
                { header: "Version" },
                { header: "File Extension"},
                { header: "Status"},
                { header: "Details"}
            ]}
            data={languages}
            renderRow={(language) => (
                <tr key={language.langId} className="border-y border-gray-500">
                    <td className="px-4 py-2 border-r border-gray-500">{language.name}</td>
                    <td className="px-4 py-2 border-r border-gray-500">{language.version}</td>
                    <td className="px-4 py-2 border-r border-gray-500">{language.fileExtension}</td>
                    <td className="px-4 py-2 border-r border-gray-500">{language.isEnabled ? <p className="text-green-400">Active</p> : <p className="text-red-500">Disabled</p>}</td>
                    <td className="px-4 py-2 border-r border-gray-500"><span className="text-blue-500 font-black hover:text-blue-300 transition duration-100"><Link to={`${language.langId}`}>View</Link></span></td>
                </tr>
            )}
        />
    )
}