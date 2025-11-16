import useCodeEditor from "../hooks/useCodeEditor"

export default function CodeEditorPage() {
    const {selectedLanguage, code, languageDropdown, handleLanguageChange, handleDropdownClick} = useCodeEditor();
    return(
        <>
            <div className="w-full h-1/14 flex items-center border-gray-300 border-b">
                <button type="button" onClick={handleDropdownClick}
                className={`flex items-center relative h-full left-1/30 bg-gray-700 rounded-lg w-1/6 wrap-anywhere cursor-pointer
                ${languageDropdown ? "rounded-b-none" : ""}`}>
                    <span className="capitalize absolute left-1/8 font-black">{selectedLanguage}</span>
                    <span className="material-symbols-outlined absolute right-1/20">
                        arrow_drop_down
                    </span>
                    <div className={`${languageDropdown ? "block" : "hidden"}
                        bg-gray-700 absolute top-full w-full`}>
                        <div onClick={() => handleLanguageChange("c#")}
                        className={`hover:bg-white hover:text-black transition duration-300 py-1
                        ${selectedLanguage === "c#" ? "bg-white text-black" : ""}`}>
                            <p className="text-left relative left-1/8 font-black">C#</p>
                        </div>
                        <div onClick={() => handleLanguageChange("java")}
                        className={`hover:bg-white hover:text-black transition duration-300 py-1
                        ${selectedLanguage === "java" ? "bg-white text-black" : ""}`}>
                            <p className="text-left relative left-1/8 font-black">Java</p>
                        </div> 
                    </div>
                </button>
            </div>
            <div className="bg-blue-400 w-full">
                <p>{code}</p>
            </div>
        </>
    )
}