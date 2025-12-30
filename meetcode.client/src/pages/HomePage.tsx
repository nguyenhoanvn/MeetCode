import NavigationBar from "../components/NavigationBar";

export default function HomePage() {
    return (
        <div className=" bg-[#161b22]">
            <NavigationBar/>
            <div className="grid grid-cols-2">
                <div className="pl-30 py-50">
                    <p className="text-2xl font-black leading-10"><span className="text-blue-500 font-black text-4xl">MeetCode</span><br/>
                    Ready to dive in?
                    </p>
                </div>
                <div className="pr-30 py-50 relative">
                    
                </div> 
            </div>
        </div>
    );
}