import { Link, useParams } from "react-router-dom";
import useTagDetail from "../hooksAdmin/useTagDetail";
import LoadingOverlay from "../components/LoadingOverlay";

export default function TagDetailPage() {
    const { id } = useParams<{ id: string }>();
    const { error, loading, tag } = useTagDetail(id!);

    return (
        <>
            {loading && (
                <LoadingOverlay message="Loading tag..."/>
            )}
            <div className="h-full w-full flex flex-col p-5 pb-10 gap-10">
                <div className="flex items-center">
                    <div className="">
                        <p className="text-lg uppercase text-gray-400 font-light">Tag Detail</p>
                        <p className="text-3xl capitalize text-white">{tag?.name}</p>
                    </div>
                </div>
                {error ? (
                    <p className="text-red-500"><span className="font-bold">Error: </span>{error}</p>
                ) : (<></>)}

                <div className="grid grid-cols-2 gap-10">
                    <div className="flex flex-col gap-3 col-span-2">
                        <p className="font-medium text-lg">ID</p>
                        <p className="font-light border-gray-500 border p-3 rounded-lg max-w-fit">{tag?.tagId}</p>
                    </div>
                    <div className="flex flex-col gap-3">
                        <p className="font-medium text-lg">Name</p>
                        <p className="font-light border-gray-500 border p-3 rounded-lg max-w-fit">{tag?.name}</p>
                    </div>
                    <div className="flex flex-col gap-3">
                        <p className="font-medium text-lg">Problem</p>
                        <Link className="w-fit" to={`/admin/problems/`}><p className="font-light bg-blue-900 hover:bg-blue-950 border-gray-500 border p-3 rounded-lg max-w-fit">{tag?.problems.length} problems</p></Link>
                    </div>
                </div>
            </div>
        </>
    )
}