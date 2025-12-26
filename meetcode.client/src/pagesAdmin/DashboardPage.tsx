import AdminSidebar from "../components/AdminSidebar";
import LoadingOverlay from "../components/LoadingOverlay";
import useDashboard from "../hooksAdmin/useDashboard";


export default function DashboardPage() {
    const { error, loading, user, pageLoading } = useDashboard();

    return (
        <>
            {pageLoading && (
                <LoadingOverlay message="Loading dashboard..." />
            )}
            <div className="h-full w-full flex flex-col p-5 pb-10 gap-10">
                <div className="flex items-center">
                    <div className="">
                        <p className="text-lg uppercase text-gray-400 font-light">Overview</p>
                        <p className="text-3xl capitalize text-white">Admin Dashboard</p>
                    </div>
                    <div className="ml-auto">
                        <p>Welcome {user?.displayName}</p>
                    </div>
                </div>
                {error ? (
                    <p className="text-red-500"><span className="font-bold">Error: </span>{error}</p>
                ) : (<></>)}

                <div className="grid grid-cols-2 gap-10">
                    
                </div>
            </div>
        </>
    )
}