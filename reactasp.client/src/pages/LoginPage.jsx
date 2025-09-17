import LoginForm from "../components/LoginForm";
import {useAuth} from "../hooks/useAuth";

export default function LoginPage() {
    const {handleLogin} = useAuth();
    return <LoginForm onLogin={handleLogin}/>;
}