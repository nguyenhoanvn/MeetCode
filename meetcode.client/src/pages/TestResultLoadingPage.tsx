export default function TestResultLoadingPage() {
    return (
        <div className="h-full px-5 py-5 flex flex-col gap-5 overflow-hidden min-h-0 animate-pulse">
            <div className="h-10 w-50 rounded-lg flex flex-row items-center gap-5 bg-gray-700"/>
            <div className="flex flex-row gap-3 bg-red">
                {Array.from({ length: 3 }).map((_, i) => (
                    <div key={i} className="h-7 w-20 flex flex-colrelative group bg-gray-700 rounded-lg">
                    </div>
                ))}
            </div>
            {Array.from({ length: 2 }).map((_, i) => (
                <div key={i} className="flex flex-col gap-3">
                    <p className="text-sm text-gray-400 bg-gray-700 h-5 w-15 rounded-lg"/>
                    <div className="flex flex-col px-3 py-3 bg-gray-700 text-sm rounded-lg gap-1">
                        <p className="text-sm text-white"/>
                    </div>
                </div>
            ))}
            
        </div>
    );
}