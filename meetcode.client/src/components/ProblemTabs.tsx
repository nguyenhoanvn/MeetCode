import { useState } from "react";

interface TabProps {
    tabs: string[];
    initialTab?: number;
    onTabChange?: (index:number) => void;
}

export default function ProblemTabs({ tabs, initialTab = 0, onTabChange }: TabProps) {
    const [selectedTab, setSelectedTab] = useState(initialTab);

    const handleClick = (index: number) => {
        setSelectedTab(index);
        if (onTabChange) onTabChange(index);
    };

    return (
    <div className="w-full h-1/14 flex items-center border-gray-300 border-b">
        <ul className="flex w-11/12 text-sm h-full">
            {tabs.map((tab, index) => (
                <li 
                key={tab}
                onClick={() => handleClick(index)}

                className={`
                cursor-pointer transition flex items-center justify-center px-4
                ${selectedTab === index ? "text-white border-b" : "text-gray-300"}
                `}
                >
                    {tab}
                </li>
            ))}
        </ul>
        <div> 
            {selectedTab === 0 && <p>Currently chosen Question</p>}
            {selectedTab === 1 && <p>Currently chosen Solution</p>}
            {selectedTab === 2 && <p>Currently chosen Submission</p>}
            {selectedTab === 3 && <p>Currently chosen Note</p>}
        </div>
    </div>
  );
}