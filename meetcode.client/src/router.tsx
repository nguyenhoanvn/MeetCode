import {BrowserRouter, Routes, Route} from "react-router-dom";
import RegisterPage from "./pages/RegisterPage";
import HomePage from "./pages/HomePage";
import LoginPage from "./pages/LoginPage";
import ProblemListPage from "./pages/ProblemListPage";
import ForgotPasswordPage from "./pages/ForgotPasswordPage";

export default function AppRouter() {
    return (
        <BrowserRouter>
            <Routes>
                <Route path="/" element={<HomePage/>}/>
                <Route path="/auth/register" element={<RegisterPage/>}/>
                <Route path="/auth/login" element={<LoginPage/>}/> 
                <Route path="/problems" element={<ProblemListPage/>}/>
                <Route path="/auth/forgot-password" element={<ForgotPasswordPage/>}/>
            </Routes>
        </BrowserRouter>
    );
}
