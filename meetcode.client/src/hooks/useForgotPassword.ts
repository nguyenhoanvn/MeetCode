import { useState } from "react"
import { forgotPassword, resetPassword, verifyOtp } from "../api/auth";
import { useNavigate } from "react-router-dom";

interface ForgotPasswordForm {
    email: string;
}

interface OtpForm {
    code: string;
}

export default function useResetPassword() {
    const [forgotForm, setForgotForm] = useState<ForgotPasswordForm>({email: ""});
    const [otpForm, setOtpForm] = useState<OtpForm>({ code: ""});
    const [loading, setLoading] = useState<boolean>(false);
    const [error, setError] = useState<string>("");
    const [message, setMessage] = useState<string>("");

    const navigate = useNavigate();

    const handleForgotFormChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const {name, value} = e.target;
        setForgotForm((prev) => ({...prev, [name]: value}));
        setMessage("");
        setError("");
    }

    const handleOtpFormChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const {name, value} = e.target;
        setOtpForm((prev) => ({...prev, [name]: value}));
        setMessage("");
        setError("");
    }

    const handleForgotPasswordSubmit = async () => {
        try {
            setLoading(true);
            const response = await forgotPassword(forgotForm);
            setMessage(response.message);
            console.log(response);
        } catch (err: any) {
            setError(err.message || "Unexpected exception happens while calling forgot password endpoint");
        } finally {
            setLoading(false);
        }
    }

    const handleVerifyOTPSubmit = async () => {
        try {
            setLoading(true);
            const requestPayload = {
                email: forgotForm.email,
                code: otpForm.code
            };
            const response = await verifyOtp(requestPayload);
            if (response.isSuccess) {
                sessionStorage.setItem("resetEmail", forgotForm.email);
                navigate("/auth/reset-password");
            } else {
                setMessage("Incorrect Otp");
            }
        } catch (err: any) {
            setError(err.message || "Unexpected exception happens while verifying OTP");
        } finally {
            setLoading(false);
        }
    }

    return { forgotForm, otpForm, loading, error, message,
        handleForgotFormChange, handleOtpFormChange, 
        handleForgotPasswordSubmit, handleVerifyOTPSubmit };
}