import React, { useEffect, useState } from "react";
import { Problem } from "../types/user/problem";

export default function useCodeEditor(problem?: Problem) {

    const [templates, setTemplates] = useState<Map<string, string>>(new Map());

    const [selectedLanguage, setSelectedLanguage] = useState(() =>
        sessionStorage.getItem("selectedLanguage") || "csharp"
    );

    const [code, setCode] = useState("");

    const [languageDropdown, setLanguageDropdown] = useState(false);

    useEffect(() => {
        if (!problem) return;

        const map = new Map(
            problem.problemTemplates.map(t => [t.languageName.toLowerCase(), t.templateCode])
        );

        setTemplates(map);
    }, [problem]);

    useEffect(() => {
        const template = templates.get(selectedLanguage);
        if (template !== undefined) setCode(template);
    }, [templates, selectedLanguage]);

    const handleDropdownClick = () => {
        setLanguageDropdown(v => !v);
    };

    const handleLanguageChange = (lang: string) => {
        setSelectedLanguage(lang);
        setLanguageDropdown(false);
        sessionStorage.setItem("selectedLanguage", lang);
    };

    return {
        selectedLanguage,
        code,
        languageDropdown,
        handleLanguageChange,
        handleDropdownClick
    };
}
