import { useState } from "react"
import { forgotPassword } from "../api/auth";

interface ForgotPasswordForm {
    email: string;
}

export default function useResetPassword() {
    const [forgotForm, setForgotForm] = useState<ForgotPasswordForm>({email: ""});
    const [loading, setLoading] = useState<boolean>(false);
    const [error, setError] = useState<string>("");
    const [message, setMessage] = useState<string>("");

    const handleForgotPasswordChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const {name, value} = e.target;
        setForgotForm((prev) => ({...prev, [name]: value}));
        setError("");
    }

    const handleForgotPasswordSubmit = async () => {
        try {
            setLoading(true);
            const response = await forgotPassword(forgotForm);
            setMessage(response);
        } catch (err: any) {
            setError(err.message || "Unexpected exception happens while calling forgot password endpoint");
        } finally {
            setLoading(false);
        }
    }

    const handleResetPasswordChange = () => {}

    const handleResetPasswordSubmit = async () => {}

    return {forgotForm, loading, error, message, handleForgotPasswordChange, handleForgotPasswordSubmit, handleResetPasswordChange, handleResetPasswordSubmit};
}