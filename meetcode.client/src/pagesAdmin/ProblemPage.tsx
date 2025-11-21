import useProblem from "../hooksAdmin/useProblem"

export default function ProblemPage() {
    const { problems } = useProblem();
    return(
        <>
            <div className="h-full w-full flex flex-col p-5 gap-10">
                <div>
                    <p className="text-md uppercase text-gray-400 font-light">Overview</p>
                    <p className="text-2xl capitalize text-white">Problems</p>
                </div>
                
                <div className="flex flex-col relative">
                    <div className="right-0 -top-1/2 absolute flex">
                        <button className="min-w-fit h-10 flex flex-row items-center px-3 rounded-lg cursor-pointer">
                            <span className="material-symbols-outlined">
                                add
                            </span>
                            <span className="">
                                Add Problem
                            </span>
                        </button>
                    </div>
                    
                    <div className="rounded-lg shadow overflow-hidden border">
                        <table className="min-w-full table-auto border-collapse">
                            <thead className="bg-gray-600">
                                <tr className="">
                                    <th className="px-4 py-2 text-left text-white font-medium">Title</th>
                                    <th className="px-4 py-2 text-left text-white font-medium">Difficulty</th>
                                    <th className="px-4 py-2 text-left text-white font-medium">Submissions</th>
                                    <th className="px-4 py-2 text-left text-white font-medium">Details</th>
                                </tr>
                            </thead>
                            <tbody>
                                {problems.map(item => (
                                    <tr key={item.problemId} className="border-t">
                                        <td className="px-4 py-2">{item.title}</td>
                                        <td className="px-4 py-2 capitalize">{item.difficulty}</td>
                                        <td className="px-4 py-2">{item.totalSubmissionCount}</td>
                                        <td className="px-4 py-2">
                                            <button className="text-blue-600 hover:underline">View</button>
                                        </td>
                                    </tr>
                                ))}
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
        </>
    )
}