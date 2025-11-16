import useCodeEditor from "../hooks/useCodeEditor"
import { Editor } from "@monaco-editor/react";

export default function CodeEditorPage() {
    const {selectedLanguage, code, languageDropdown, handleLanguageChange, handleDropdownClick} = useCodeEditor();
    return(
        <>
            <div className="w-full h-1/14 flex items-center border-gray-300 border-b relative">
                <button type="button" onClick={handleDropdownClick}
                className={`flex items-center relative h-full left-1/30 bg-gray-700 rounded-lg w-1/6 wrap-anywhere cursor-pointer
                ${languageDropdown ? "rounded-b-none" : ""}`}>
                    <span className="capitalize absolute left-1/8 font-black">{selectedLanguage}</span>
                    <span className="material-symbols-outlined absolute right-1/20">
                        arrow_drop_down
                    </span>
                </button>
                <div className={`${languageDropdown ? "block" : "hidden"}
                    bg-gray-700 absolute left-1/30 top-full w-1/6 z-10`}>
                    <div onClick={(e) => {
                        e.stopPropagation();
                        handleLanguageChange("c#")}}
                    className={`hover:bg-white hover:text-black transition duration-300 py-1
                    ${selectedLanguage === "c#" ? "bg-white text-black" : ""}`}>
                        <p className="text-left relative left-1/8 font-black">C#</p>
                    </div>
                    <div onClick={(e) => {
                        e.stopPropagation();
                        handleLanguageChange("java")}}
                    className={`hover:bg-white hover:text-black transition duration-300 py-1
                    ${selectedLanguage === "java" ? "bg-white text-black" : ""}`}>
                        <p className="text-left relative left-1/8 font-black">Java</p>
                    </div> 
                </div>
            </div>
            <div>
                <Editor height="100%"
                language={selectedLanguage}
                value={code}
                theme="vs-dark"
                options={{
                    quickSuggestions: false,
                    suggestOnTriggerCharacters: false,
                    wordBasedSuggestions: "off",
                    parameterHints: { enabled: false },
                    tabCompletion: "off",

                    renderControlCharacters: false,
                    renderWhitespace: "none",
                    lineNumbers: "on",
                    contextmenu: false,
                    minimap: { enabled: false},

                    fontSize: 16,
                    padding: { top: 10}
                }}
                />
            </div>
        </>
    )
}