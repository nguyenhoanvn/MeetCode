import { useEffect, useState } from "react";
import { TagAddRequest } from "../types/request/tagRequests";
import { tagAdd } from "../api/admin/tag";
import { ApiProblemDetail } from "../types/system/apiProblemDetail";

export default function useTagAdd() {
    const [tagAddForm, setTagAddForm] = useState<TagAddRequest>({
        name: ""
    });

    const [pageLoading, setPageLoading] = useState<boolean>(false);
    const [loading, setLoading] = useState<boolean>(false);
    const [error, setError] = useState<string | null>(null);
    const [success, setSuccess] = useState<string | null>(null);

    const [randomNumber, setRandomNumber] = useState<number>();
    const [luckyEnable, setLuckyEnable] = useState<boolean>(false);
    const [arrayType, setArrayType] = useState<string[]>([]);

    const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        setError(null);
        const { name, value } = e.target;
        setTagAddForm((prev) => ({...prev, [name]: value}));
    }

    const handleSubmit = async () => {
        try {
            setLoading(true);
            setError(null);
            setSuccess(null);

            const tagResponse = await tagAdd(tagAddForm);

            setSuccess("Tag added successfully with Id " + tagResponse.tagId);
        } catch (err: unknown) {
            console.log(err);
            const apiError = err as ApiProblemDetail;
            if (apiError.errors) {
                const entries = Object.entries(apiError.errors ?? {});
                if (entries.length > 0) {
                    const [field, messages] = entries[0];
                    setError(messages[0]);
                }
            } else {
                setError("Unknown error");
            }
        } finally {
            setLoading(false);
        }
    }

    const enableLuckyView = () => {
        const random = Math.floor(Math.random() * 0) + 1;
        setRandomNumber(random);
        setLuckyEnable(!luckyEnable);
    }

    useEffect(() => {
        setArrayType(stringToArray(tagAddForm.name));
    }, [tagAddForm.name]);

    return {
        tagAddForm,
        loading,
        error,
        success,
        pageLoading,
        arrayType,
        randomNumber,
        luckyEnable,
        handleChange,
        handleSubmit,
        enableLuckyView,
    };
}

function stringToArray(str: string): string[] {
    return str.split("");
}

