import React, { useEffect, useState } from "react";

const templates = new Map([
    ["c#", "public class Solution {\n\t{nameOfMethod}\n}"],
    ["java", "class Solution {\n\t{nameOfMethod}\n}"],
]);

export default function useCodeEditor() {
    const [selectedLanguage, setSelectedLanguage] = useState<string>(() => {
        return sessionStorage.getItem("selectedLanguage") || "c#";
    })

    const [code, setCode] = useState<string>(() => {
        return templates.get(selectedLanguage) || "";
    });

    const [languageDropdown, setLanguageDropdown] = useState<boolean>(false);

    const handleDropdownClick = () => {
        setLanguageDropdown(languageDropdown => !languageDropdown);
    }

    const handleLanguageChange = (input: string) => {
        setSelectedLanguage(input);
        setCode(templates.get(input) || "");
        sessionStorage.setItem("selectedLanguage", input);
    }

    return {selectedLanguage, code, languageDropdown, handleLanguageChange, handleDropdownClick};
}