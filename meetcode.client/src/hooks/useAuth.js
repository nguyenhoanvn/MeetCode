import { useEffect, useState } from "react";
import { register } from "../api/auth";
import { login } from "../api/auth";

export const useRegister = () => {
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState(null);

    const registerUser = async (data) => {
        try {
            setLoading(true);
            const response = await register(data);
            return response.data;
        } catch (err) {
            setError(err);
        } finally {
            setLoading(false);
        }
    }

    return {registerUser, loading, error};
}

export const useLogin = () => {
    const [response, setResponse] = useState();
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);

    useEffect(() => {
        login()
            .then((res) = setResponse(res.data))
            .catch((err) => setError(err))
            .finally(() => setLoading(false))
    }, );

    return {response, loading, error};
}