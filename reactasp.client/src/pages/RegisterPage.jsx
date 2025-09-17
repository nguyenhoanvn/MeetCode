import RegisterForm from "../components/RegisterForm";
import { useAuth } from "../hooks/useAuth";

export default function RegisterPage() {
    const {handleRegister} = useAuth();
    return <RegisterForm onRegister={handleRegister}/>
}