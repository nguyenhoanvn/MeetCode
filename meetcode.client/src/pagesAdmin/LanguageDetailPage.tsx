import { Link, useParams } from "react-router-dom";
import useLanguageDetail from "../hooksAdmin/useLanguageDetail";
import LoadingOverlay from "../components/LoadingOverlay";

export default function LanguageDetailPage() {
    const { id } = useParams<{ id: string }>();
    const { language, loading, error, toggleLoading, toggleStatus } = useLanguageDetail(id!);

    return (
        <>
            {loading && (
                <LoadingOverlay message="Loading language..."/>
            )}
            <div className="h-full w-full flex flex-col p-5 pb-10 gap-10">
                <div className="flex items-center">
                    <div className="">
                        <p className="text-lg uppercase text-gray-400 font-light">Language Detail</p>
                        <p className="text-3xl capitalize text-white">{language?.name}</p>
                    </div>
                    <div className="ml-auto">
                        <button
                            disabled={toggleLoading}
                            onClick={() => toggleStatus()}
                            className={`px-4 py-2 w-35 border-gray-400 border rounded-lg text-sm font-medium bg-blue-900
                        ${language?.isEnabled ? "hover:bg-cyan-900" : "hover:bg-purple-950"} hover:cursor-pointer`}
                        >
                            {toggleLoading ? (
                                <div className="flex gap-3 items-center justify-center">
                                    <span className="material-symbols-outlined animate-spin">
                                        progress_activity
                                    </span>
                                    <p className="">Loading...</p>
                                </div>
                            ) : language?.isEnabled ?
                                (<div className="flex gap-3 items-center justify-center">
                                    <img className="h-4 w-4 animate-pulse"
                                        src="/images/green-dot.png" />
                                    <p className="text-green-400 animate">Active</p>
                                </div>) :
                                (<div className="flex gap-3 items-center justify-center">
                                    <img className="h-4 w-4 animate-pulse"
                                        src="/images/red-dot.png" />
                                    <p className="text-red-500">Disabled</p>
                                </div>)}


                        </button>
                    </div>
                </div>
                {error ? (
                    <p className="text-red-500"><span className="font-bold">Error: </span>{error}</p>
                ) : (<></>)}

                <div className="grid grid-cols-2 gap-10">
                    <div className="flex flex-col gap-3">
                        <p className="font-medium text-lg">ID</p>
                        <p className="font-light border-gray-500 border p-3 rounded-lg max-w-fit">{language?.langId}</p>
                    </div>
                    <div className="flex flex-col gap-3">
                        <p className="font-medium text-lg">Name</p>
                        <p className="font-light border-gray-500 border p-3 rounded-lg max-w-fit">{language?.name}</p>
                    </div>
                    <div className="flex flex-col gap-3">
                        <p className="font-medium text-lg">Version</p>
                        <p className="font-light border-gray-500 border p-3 rounded-lg max-w-fit">{language?.version}</p>
                    </div>
                    <div className="flex flex-col gap-3">
                        <p className="font-medium text-lg">File Extension</p>
                        <p className="font-light border-gray-500 border p-3 rounded-lg max-w-fit">{language?.fileExtension}</p>
                    </div>
                    <div className="flex flex-col gap-3">
                        <p className="font-medium text-lg">Compile Image</p>
                        <p className="font-light border-gray-500 border p-3 rounded-lg max-w-fit">{language?.compileImage}</p>
                    </div>
                    <div className="flex flex-col gap-3">
                        <p className="font-medium text-lg">Runtime Image</p>
                        <p className="font-light border-gray-500 border p-3 rounded-lg max-w-fit">{language?.runtimeImage}</p>
                    </div>
                    <div className="flex flex-col gap-3">
                        <p className="font-medium text-lg">Compile Command</p>
                        <p className="font-light border-gray-500 border p-3 rounded-lg max-w-fit">{language?.compileCommand}</p>
                    </div>
                    <div className="flex flex-col gap-3">
                        <p className="font-medium text-lg">Run Command</p>
                        <p className="font-light border-gray-500 border p-3 rounded-lg max-w-fit">{language?.runCommand}</p>
                    </div>
                    <div className="flex flex-col gap-3">
                        <p className="font-medium text-lg">Submissions</p>
                        <Link className="w-fit" to={`/admin/languages/${language?.langId}`}><p className="font-light bg-blue-900 hover:bg-blue-950 border-gray-500 border p-3 rounded-lg max-w-fit">{language?.submissions.length} {language?.submissions.length === 1 ? "submission" : "submissions"}</p></Link>
                    </div>
                    <div className="flex flex-col gap-3">
                        <p className="font-medium text-lg">Problem Templates</p>
                        <Link className="w-fit" to={`/admin/problem-templates`}><p className="font-light bg-blue-900 hover:bg-blue-950 border-gray-500 border p-3 rounded-lg max-w-fit">{language?.problemTemplates.length} {language?.problemTemplates.length === 1 ? "template" : "templates"}</p></Link>
                    </div>
                </div>
            </div>
        </>
    )
}