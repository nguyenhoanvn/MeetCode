import { useState } from "react";

export default function ForbiddenPage() {

    const [randomNumber] = useState(() => Math.floor(Math.random() * 5) + 1);

    const quotesMap = new Map<number, string>([
        [1, "You don't have permission to access this resource. This could be due to insufficient privileges or restricted access."],
        [2, "Nice try! But you need to break this barrier first."],
        [3, "Oops! Sorry for banging this door right at your head, now head back."],
        [4, "Got cha, it's not that easy to access to that place little boy."],
        [5, "It seems like where you suppose to go is locked. Send 100 Bitcoin to unlock it."]
    ]);


    return (
        <>
            <div className="h-screen w-full flex justify-center items-center text-center">
                <div className="h-fit w-fit">
                    <div className="mb-8 flex justify-center bg-red">
                        <div className="relative">
                            <svg className="w-32 h-32 text-white animate-pulse" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="1.5" d="M12 15v2m-6 4h12a2 2 0 002-2v-6a2 2 0 00-2-2H6a2 2 0 00-2 2v6a2 2 0 002 2zm10-10V7a4 4 0 00-8 0v4h8z" />
                            </svg>
                            <div className="absolute inset-0 bg-yellow-300 opacity-20 blur-2xl rounded-full"></div>
                        </div>
                    </div>


                    <h1 className="text-8xl font-bold text-transparent bg-clip-text bg-gradient-to-r from-blue-500 to-cyan-300 mb-4">
                        403
                    </h1>


                    <h2 className="text-3xl font-semibold text-gray-100 mb-4">
                        {randomNumber === 4 ? ("Admin Dashboard") : ("Access forbidden")}
                    </h2>


                    <p className="text-gray-400 text-lg mb-8 leading-relaxed">
                        {quotesMap.get(randomNumber)}
                    </p>

                    <div className="flex flex-col sm:flex-row gap-4 justify-center items-center">
                        <a href="/" className="px-8 py-3 bg-gray-800 text-gray-100 font-semibold rounded-lg hover:bg-gray-700 transition-all duration-300 border border-gray-700 hover:border-gray-600 transform hover:scale-105">
                            Return Home
                        </a>
                    </div>
                </div>
            </div>

            <div className="fixed top-0 left-0 w-full h-full overflow-hidden pointer-events-none -z-10">
                <span className="material-symbols-outlined absolute top-10 left-20 text-blue-500 text-9xl! blur-md">question_mark</span>
                <span className="material-symbols-outlined absolute top-40 right-32 text-blue-500 text-9xl! blur-md">question_mark</span>
                <span className="material-symbols-outlined absolute bottom-32 left-10 text-blue-500 text-9xl! blur-md">question_mark</span>
                <span className="material-symbols-outlined absolute top-1/4 left-1/2 text-blue-500 text-9xl! blur-md">question_mark</span>
                <span className="material-symbols-outlined absolute bottom-20 right-20 text-blue-500 text-9xl! blur-md">question_mark</span>
                <span className="material-symbols-outlined absolute top-60 left-1/3 text-blue-500 text-9xl! blur-md">question_mark</span>
                <span className="material-symbols-outlined absolute bottom-1/3 right-1/4 text-blue-500 text-9xl! blur-md">question_mark</span>
                <span className="material-symbols-outlined absolute top-1/3 right-10 text-blue-500 text-9xl! blur-md">question_mark</span>
            </div>
        </>
    )
}