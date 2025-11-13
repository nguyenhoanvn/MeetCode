import { use, useState } from "react"
import { forgotPassword, resetPassword, verifyOtp } from "../api/auth";
import { useNavigate } from "react-router-dom";

interface ForgotPasswordForm {
    email: string;
}

interface OtpForm {
    code: string;
}

interface ForgotPasswordResponse {
    isSuccess: boolean;
}

interface SendOTPField {
    message: string;
    status: boolean;
    cooldown: number;
}

interface VerifyOTPField {
    message: string;
    status: boolean;
}

export default function useForgotPassword() {
    const [forgotForm, setForgotForm] = useState<ForgotPasswordForm>({email: ""});
    const [otpForm, setOtpForm] = useState<OtpForm>({ code: ""});
    const [loading, setLoading] = useState<boolean>(false);
    const [sendOtpButton, setSendOtpButton] = useState<SendOTPField>({message: "", status: false, cooldown: 0});
    const [verifyOtpButton, setVertifyOtpButton] = useState<VerifyOTPField>({message: "", status: false});
    const [error, setError] = useState<string>("");

    const navigate = useNavigate();

    const handleForgotFormChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const {name, value} = e.target;

        setForgotForm((prev) => ({...prev, [name]: value}));
        setSendOtpButton((prev) => ({...prev, message: ""}));
        setError("");
    }

    const handleOtpFormChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const {name, value} = e.target;

        if (!/^\d*$/.test(value)) {
            setVertifyOtpButton((prev) => ({...prev, message: "OTP can only have digits"}));         
        } else {
            setVertifyOtpButton((prev) => ({...prev, message: ""}));
        }
        setOtpForm((prev) => ({...prev, [name]: value}));
        setError("");
    }

    const handleForgotPasswordSubmit = async () => {
        try {
            setSendOtpButton((prev) => ({...prev, status: true, cooldown: 30}));

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

            const response = await forgotPassword(forgotForm);
            if (!response.isSuccess) {
                setSendOtpButton((prev) => ({...prev, message: "Failed to sent verification code"}));
            }
            console.log(response);
        } catch (err: any) {
            setSendOtpButton((prev) => ({...prev, message: "Unexpected exception happens while calling forgot password endpoint"}));
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
            const response = await verifyOtp(requestPayload);
            if (response.isSuccess) {
                sessionStorage.setItem("resetEmail", forgotForm.email);
                navigate("/auth/reset-password");
            } else {
                setVertifyOtpButton((prev) => ({...prev, message: "Incorrect OTP"}));
            }
        } catch (err: any) {
            setVertifyOtpButton((prev) => ({...prev, message:  "Unexpected exception happens while verifying OTP"}));
        } finally {
            setLoading(false);
        }
    }

    return { forgotForm, otpForm, loading, error, sendOtpButton, verifyOtpButton,
        handleForgotFormChange, handleOtpFormChange, 
        handleForgotPasswordSubmit, handleVerifyOTPSubmit };
}