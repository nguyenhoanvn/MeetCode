import {BrowserRouter, Routes, Route} from "react-router-dom";
import RegisterPage from "./pages/RegisterPage";
import HomePage from "./pages/HomePage";
import LoginPage from "./pages/LoginPage";
import ProblemListPage from "./pages/ProblemListPage";
import ForgotPasswordPage from "./pages/ForgotPasswordPage";
import ResetPasswordPage from "./pages/ResetPasswordPage";
import DashboardPage from "./pagesAdmin/DashboardPage";
import AdminLayout from "./pagesAdmin/Layout";
import ProblemPage from "./pagesAdmin/ProblemPage";
import LanguagePage from "./pagesAdmin/LanguagePage";
import LanguageDetailPage from "./pagesAdmin/LanguageDetailPage";
import ProblemTemplatePage from "./pagesAdmin/ProblemTemplatePage";
import ProblemTemplateDetailPage from "./pagesAdmin/ProblemTemplateDetailPage";
import ProblemDetailPage from "./pagesAdmin/ProblemDetailPage";
import TagPage from "./pagesAdmin/TagPage";
import ProblemAddPage from "./pagesAdmin/ProblemAddPage";

export default function AppRouter() {
    return (
        <BrowserRouter>
            <Routes>
                <Route path="/" element={<HomePage/>}/>
                <Route path="/auth/register" element={<RegisterPage/>}/>
                <Route path="/auth/login" element={<LoginPage/>}/> 
                <Route path="/problems" element={<ProblemListPage/>}/>
                <Route path="/auth/forgot-password" element={<ForgotPasswordPage/>}/>
                <Route path="/auth/reset-password" element={<ResetPasswordPage/>}/>
                <Route path="/problems/:slug" element={<ProblemDetailPage/>}/>
                <Route path="/admin" element={<AdminLayout />}>
                    <Route index element={<DashboardPage />} />
                    <Route path="languages" element={<LanguagePage />} />
                    <Route path="languages/:id" element={<LanguageDetailPage/>}/>
                    <Route path="problems" element={<ProblemPage />} />
                    <Route path="problems/:id" element={<ProblemDetailPage/>}/>
                    <Route path="problems/add" element={<ProblemAddPage/>}/>
                    <Route path="problem-templates" element={<ProblemTemplatePage />} />
                    <Route path="problem-templates/:id" element={<ProblemTemplateDetailPage />}/>
                    <Route path="tags" element={<TagPage/>}/>
                </Route>
            </Routes>
        </BrowserRouter>
    );
}
