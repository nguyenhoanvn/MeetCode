import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { ApiProblemDetail } from "../types/system/apiProblemDetail";
import { resetPassword } from "../api/user/auth";

interface ResetPasswordForm {
    newPassword: string;
    confirmPassword: string;
}

export default function useResetPassword() {
    const [resetPasswordForm, setResetPasswordForm] = useState<ResetPasswordForm>({newPassword: "", confirmPassword: ""});
    const [loading, setLoading] = useState<boolean>(false);
    const [error, setError] = useState<string | null>("");

    const navigate = useNavigate();

    const handleResetPasswordFormChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const {name, value} = e.target;
        setResetPasswordForm(prev => {
            const updatedForm = {...prev, [name]: value};
            if (updatedForm.confirmPassword && updatedForm.newPassword !== updatedForm.confirmPassword) {
                setError("Passwords do not match");
            } else {
                setError(null);
            }
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
            await resetPassword(requestPayload);
            sessionStorage.removeItem("resetEmail");
            navigate("/auth/login");

        } catch (err: unknown) {
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

    return {resetPasswordForm, loading, error, handleResetPasswordFormChange, handleResetPasswordSubmit};
}