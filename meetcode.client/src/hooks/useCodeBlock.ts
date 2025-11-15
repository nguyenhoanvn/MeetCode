import { useState } from "react";


export default function useCodeBlock(children: string) {
    const [copied, setCopied] = useState<boolean>(false);

    const handleCopyToClipboard = async () => {
        await navigator.clipboard.writeText(children);
        setCopied(true);
        setTimeout(() => setCopied(false), 1000);
    };

    return {copied, handleCopyToClipboard};
}