import { use, useState } from "react"
import { forgotPassword, resetPassword, verifyOtp } from "../api/auth";
import { useNavigate } from "react-router-dom";
import { ApiProblemDetail } from "../types/system/apiProblemDetail";

interface ForgotPasswordForm {
    email: string;
}

interface OtpForm {
    code: string;
}

interface SendOTPField {
    status: boolean;
    cooldown: number;
}

interface VerifyOTPField {
    status: boolean;
}

export default function useForgotPassword() {
    const [forgotForm, setForgotForm] = useState<ForgotPasswordForm>({email: ""});
    const [otpForm, setOtpForm] = useState<OtpForm>({ code: ""});
    const [loading, setLoading] = useState<boolean>(false);
    const [sendOtpButton, setSendOtpButton] = useState<SendOTPField>({ status: false, cooldown: 0});
    const [verifyOtpButton, setVertifyOtpButton] = useState<VerifyOTPField>({ status: false});
    const [error, setError] = useState<string | null>(null);
    const [errorField, setErrorField] = useState<string | null>(null);

    const navigate = useNavigate();

    const handleForgotFormChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const {name, value} = e.target;

        setForgotForm((prev) => ({...prev, [name]: value}));
        setError(null);
        setErrorField(null);
    }

    const handleOtpFormChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const {name, value} = e.target;

        if (!/^\d*$/.test(value)) {
            setVertifyOtpButton((prev) => ({...prev, message: "OTP can only have digits"}));         
        } else {
            setVertifyOtpButton((prev) => ({...prev, message: ""}));
        }
        setOtpForm((prev) => ({...prev, [name]: value}));
        setError(null);
        setErrorField(null);
    }

    const handleForgotPasswordSubmit = async () => {
        try {
            setSendOtpButton((prev) => ({...prev, status: true, cooldown: 10}));

            const timer = setInterval(() => {
                setSendOtpButton(prev => {
                    if (prev.cooldown <= 1) {
                        clearInterval(timer);
                        prev.status = false;
                        return {...prev, cooldown: 0 };
                    }
                    return {...prev, cooldown: prev.cooldown - 1};
                });
            }, 1000);

            await forgotPassword(forgotForm);

        } catch (err: any) {
            const apiError = err as ApiProblemDetail;
            if (apiError.errors) {
                const entries = Object.entries(apiError.errors ?? {});
                if (entries.length > 0) {
                    const [field, messages] = entries[0];      
                    setError(messages[0]); 
                    setErrorField(field);
                }
            } else {
                setError("Unknown error");
            }
        } finally {
            setLoading(false);
        }
    }

    const handleVerifyOTPSubmit = async () => {
        try {
            setVertifyOtpButton((prev) => ({...prev, status: true}));
            const requestPayload = {
                email: forgotForm.email,
                code: otpForm.code
            };
            await verifyOtp(requestPayload);
            sessionStorage.setItem("resetEmail", forgotForm.email);
            navigate("/auth/reset-password");

        } catch (err: any) {
            const apiError = err as ApiProblemDetail;
            if (apiError.errors) {
                const entries = Object.entries(apiError.errors ?? {});
                if (entries.length > 0) {
                    const [field, messages] = entries[0];      
                    setError(messages[0]);
                    setErrorField(field); 
                }
            } else {
                setError("Unknown error");
            }
        } finally {
            setVertifyOtpButton((prev) => ({...prev, status: false}));
        }
    }

    return { forgotForm, otpForm, loading, error, errorField, sendOtpButton, verifyOtpButton,
        handleForgotFormChange, handleOtpFormChange, 
        handleForgotPasswordSubmit, handleVerifyOTPSubmit };
}