type Column = {
    header: string;
    className?: string;
};

type TablePageProps<T> = {
    overview: string;
    title: string;
    addAction?: string;
    onActionClick?: () => void;
    columns: Column[];
    data: T[];
    renderRow: (item: T) => React.ReactNode;
};

export default function TablePage<T>({
    overview,
    title,
    addAction,
    onActionClick,
    columns,
    data,
    renderRow
} : TablePageProps<T>) {
    return (
        <div className="h-full w-full flex flex-col p-5 gap-10">
            {/* Header */}
            <div className="flex items-center justify-between">
                <div className="">
                    <p className="text-md uppercase text-gray-400 font-light">{overview}</p>
                    <p className="text-2xl capitalize text-white">{title}</p>
                </div>       
            </div>

            {/* Content */}
            <div className="flex flex-col gap-3">
                <div className="">
                    {addAction && (
                        <button
                            onClick={onActionClick}
                            className="h-9 flex items-center gap-2 px-3 rounded-lg
                                    bg-blue-700 hover:bg-blue-800 hover:cursor-pointer transition-colors ml-auto"
                        >
                            <span className="material-symbols-outlined">add</span>
                            <span>{addAction}</span>
                        </button>
                    )}
                </div>
            
                <div className="flex flex-col relative">
                    <div className="rounded-lg shadow overflow-hidden border h-180">
                        <div className="h-full overflow-y-auto">
                            <table className="min-w-full table-fixed border-collapse">
                                <thead className="bg-gray-600 sticky top-0 z-10">
                                    <tr className="border-b">
                                        {columns.map((col, idx) => (
                                            <th
                                                key={idx}
                                                className={`px-4 py-2 text-left text-white font-medium border-r last:border-r-0 ${col.className ?? ""}`}
                                            >
                                                {col.header}
                                            </th>
                                        ))}
                                    </tr>
                                </thead>
                                <tbody>
                                    {data.length > 0 ? (
                                        data.map(renderRow)
                                    ) : (
                                        <tr>
                                            <td
                                                colSpan={columns.length}
                                                className="text-center text-gray-400 h-160"
                                            >
                                                <div className="flex items-center justify-center h-full">
                                                    No records found
                                                </div>
                                            </td>
                                        </tr>
                                    )}
                                </tbody>
                            </table>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
}