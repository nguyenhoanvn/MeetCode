import { useState } from "react";
import { resetPassword } from "../api/auth";
import { useNavigate } from "react-router-dom";

interface ResetPasswordForm {
    newPassword: string;
    confirmPassword: string;
}

export default function useResetPassword() {
    const [resetPasswordForm, setResetPasswordForm] = useState<ResetPasswordForm>({newPassword: "", confirmPassword: ""});
    const [loading, setLoading] = useState<boolean>(false);
    const [error, setError] = useState<string>("");
    const [message, setMessage] = useState<string>("");

    const navigate = useNavigate();

    const handleResetPasswordFormChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const {name, value} = e.target;
        setResetPasswordForm(prev => {
            const updatedForm = {...prev, [name]: value};
            if (updatedForm.confirmPassword && updatedForm.newPassword !== updatedForm.confirmPassword) {
                setMessage("Passwords do not match");
            } else {
                setMessage("");
            }

            setError("");
            return updatedForm;
        });
    }

    const handleResetPasswordSubmit = async () => {
        try {
            setLoading(true);
            const requestPayload = {
                email: sessionStorage.getItem("resetEmail") || "",
                newPassword: resetPasswordForm.newPassword
            };
            sessionStorage.removeItem("resetEmail");
            const response = await resetPassword(requestPayload);
            if (response.isSuccess) {
                navigate("/auth/login");
            } else {
                setMessage("Failed to update password");
            }
        } catch (err: any) {
            setError(err.message || "Unexpected exception happens while resetting password");
        } finally {
            setLoading(false);
        }
    }

    return {resetPasswordForm, loading, error, message, handleResetPasswordFormChange, handleResetPasswordSubmit};
}