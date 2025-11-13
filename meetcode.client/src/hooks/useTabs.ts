import { useState } from "react";

export default function useTabs(initialTab: number = 0) {
    const [selectedTab, setSelectedTab] = useState<number>(0);

    const handleSelectTab = (index: number) => {
        setSelectedTab(index);
    }

    return {selectedTab, handleSelectTab};
}