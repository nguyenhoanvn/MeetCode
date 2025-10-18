import {BrowserRouter, Routes, Route} from "react-router-dom";
import LoginPage from "./pages/LoginPage";
import RegisterPage from "./pages/RegisterPage";
import HomePage from "./pages/HomePage";

export default function AppRouter() {
    return (
        <BrowserRouter>
            <Routes>
                <Route path="/" element={<HomePage/>}/>
                <Route path="/login" element={<LoginPage/>}/>
                <Route path="/register" element={<RegisterPage/>}/>
            </Routes>
        </BrowserRouter>
    );
}
