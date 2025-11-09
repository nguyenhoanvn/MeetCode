import NavigationBar from "../components/NavigationBar";
import useProblemList from "../hooks/useProblemList";

export default function ProblemListPage() {
    const {problemList, problemSearchBox, loading, error, handleProblemList, handleSearch} = useProblemList();

    return(
        <div className="h-screen w-screen bg-amber-300 overflow-x-hidden">
            <NavigationBar/>
            <div className="h-full w-full flex flex-col pt-5">
                <div className="w-3/4 h-full bg-blue-400 self-center flex flex-col gap-10">
                    <div className="w-full h-1/12 bg-green-500 flex flex-row justify-center border-b-5 text-2xl">
                        <div className="w-1/5 h-full bg-amber-200 flex justify-center items-end">
                            <p className="relative bottom-0.5">Core Skill</p>
                        </div>
                        <div className="w-1/5 h-full bg-red-400 flex justify-center items-end">
                            <p className="relative bottom-0.5">Blind 75</p>
                        </div>
                        <div className="w-1/5 h-full bg-gray-600 flex justify-center items-end">
                            <p className="relative bottom-0.5">System Design</p>
                        </div>
                    </div>
                    <div className="bg-yellow-400 h-1/5 w-full">
                        <div className="grid grid-cols-3 h-3/4">
                            <div className="bg-red-400"></div>
                            <div className="bg-red-400 flex justify-center items-center">
                                <p className="font-bold text-2xl">6 / 75</p>
                            </div>
                            <div className="bg-green-400 flex justify-center relative items-center">
                                <div className="bg-red-500 absolute rounded-lg inset-0">
                                    <p>Info of the list</p>
                                </div>
                            </div>
                        </div>
                        <div className="h-1/4 w-full bg-gray-400 mt-3">
                            Progressive bar
                        </div>
                    </div>
                    <div className="bg-purple-500 w-full h-1/8 flex flex-col justify-center align-middle">
                        <input 
                        name="searchName"
                        type="text" 
                        placeholder="Search"
                        onChange={handleSearch}
                        value={problemSearchBox}
                        className="h-1/2 w-1/6 border-2 bg-red-300"/>
                    </div>
                    <div className="bg-red-300 h-fit w-full">
                        {problemList.map((item, index) => (
                            <div className="bg-amber-700">
                                <p>{item.problemId}</p>
                                <p>{item.title}</p>
                            </div>
                        ))}
                    </div>
                </div>
            </div>
            
        </div>
    )
}