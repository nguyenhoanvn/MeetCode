import NavigationBar from "../components/NavigationBar";
import useProblemList from "../hooks/useProblemList";
import { Link } from "react-router-dom";

export default function ProblemListPage() {
    const {problemList, problemSearchBox, loading, error, totalCount,
        handleProblemList, handleSearch} = useProblemList();

    return(
        <div className="h-screen w-screen bg-[#161b22] overflow-x-hidden">
            <NavigationBar/>
            <div className="h-full w-full flex flex-col pt-5">
                <div className="w-1/2 h-full self-center flex flex-col gap-10">
                    <div className="h-fit w-full">
                        <div className="grid grid-cols-3 h-3/4">
                            <div className=""></div>
                            <div className="flex justify-center items-center">
                                <p className="font-bold text-2xl">Total: {totalCount} problems</p>
                            </div>
                            <div className="flex justify-center relative items-center">

                            </div>
                        </div>
                    </div>
                    <div className="w-full h-fit flex flex-col justify-center px-10">
                        <div className="relative flex items-center h-1/2 w-1/2">
                            <input
                            name="searchName"
                            type="text"
                            placeholder="Search"
                            onChange={handleSearch}
                            value={problemSearchBox}
                            className="w-6/10 border-2 bg-[#161b22] pl-5 py-1 rounded-lg outline-none
                                        transition-all duration-300 ease-in-out
                                        focus:w-full"
                            />
                            <span className="material-symbols-outlined text-white text-xl relative right-1/13">
                                search
                            </span>                           
                        </div>
                    </div>

                    <div className="h-fit w-full">
                        <div className="flex flex-col items-center">
                            <div className="w-full border-collapse rounded-lg overflow-hidden">
                                {problemList.length > 0 ? (
                                    <table className="bg-gray-700 w-full">
                                    <tbody className="text-center font-bold text-md">
                                        {problemList.map((item, index) => (
                                            <tr key={item.problemId} 
                                            className="h-12 hover:bg-gray-600 transition duration-300 capitalize
                                            first:border-b last:border-t not-first:not-last:border-b not-first:not-last:border-t">
                                                <td className="w-1/2 text-start pl-15">
                                                    <Link
                                                    to={`/problems/${item.slug}`}
                                                    className="hover:text-blue-400 transition duration-300"
                                                    >
                                                        {item.title}
                                                    </Link>
                                                </td>
                                                <td className={`
                                                    ${item.difficulty === "easy" ? "text-green-500" : item.difficulty === "medium" ? "text-amber-300" : "text-red-500"} 
                                                    `}>{item.difficulty}</td>
                                                <td>{item.totalSubmissionCount}</td>
                                                <td><button>Click</button></td>
                                            </tr>
                                        ))}
                                    </tbody>
                                </table>
                                ) : (<></>)}
                                
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            
        </div>
    )
}