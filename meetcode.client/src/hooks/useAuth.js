import { useState } from "react";
import { register } from "../api/auth";
import { login } from "../api/auth";
import { useNavigate } from "react-router-dom";

export const useRegister = () => {
    const [formData, setFormData] = useState({displayName: "", email: "", password: ""});
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState(null);

    const handleChange = (e) => {
        const {name, value} = e.target;
        setFormData((prev) => ({...prev, [name]: value}));
    };

    const handleSubmit = async () => {
        setLoading(true);
        setError(null);
        try {
            await register(formData);
        } catch (err) {
            setError(err.message);
            console.log(err.message);
        } finally {
            setLoading(false);
        }
    };

    return {formData, handleChange, handleSubmit, loading, error};
}

export const useLogin = () => {
    const [resp, setResp] = useState({isSuccessful: true, message: ""})
    const [formData, setFormData] = useState({email: "", password: ""});
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState(null);

    const navigate = useNavigate();

    const handleChange = (e) => {
        const {name, value} = e.target;
        setFormData((prev) => ({...prev, [name]: value}));
    };

    const handleSubmit = async () => {
        try {
            setLoading(true); 
            const response = await login(formData);
            setResp(response);
            if (response.isSuccessfully) {
                navigate("/");
            } else {
                console.log(response.message);
            }
        } catch (err) {
            setError(err.message);
        } finally {
            setLoading(false);
        }
    }

    return {formData, resp, handleChange, handleSubmit, loading, error};
}