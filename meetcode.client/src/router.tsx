import {BrowserRouter, Routes, Route} from "react-router-dom";
import RegisterPage from "./pages/RegisterPage";
import HomePage from "./pages/HomePage";
import LoginPage from "./pages/LoginPage";
import AdminLoginPage from "./pagesAdmin/LoginPage";
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
import TagAddPage from "./pagesAdmin/TagAddPage";
import TagDetailPage from "./pagesAdmin/TagDetailPage";
import ProblemTemplateAddPage from "./pagesAdmin/ProblemTemplateAddPage";
import ForbiddenPage from "./pages/ForbiddenPage";
import AdminGuard from "./helpers/AdminGuard";

export default function AppRouter() {
    return (
        <BrowserRouter>
            <Routes>
                <Route path="/" element={<HomePage/>}/>
                <Route path="/forbidden" element={<ForbiddenPage/>}/>
                <Route path="/auth/register" element={<RegisterPage/>}/>
                <Route path="/auth/login" element={<LoginPage/>}/> 
                <Route path="/problems" element={<ProblemListPage/>}/>
                <Route path="/auth/forgot-password" element={<ForgotPasswordPage/>}/>
                <Route path="/auth/reset-password" element={<ResetPasswordPage/>}/>
                <Route path="/problems/:slug" element={<ProblemDetailPage/>}/>
                <Route element={<AdminGuard />}>
                    <Route path="/admin" element={<AdminLayout />}>
                        <Route index element={<DashboardPage />} />
                        <Route path="languages" element={<LanguagePage />} />
                        <Route path="languages/:id" element={<LanguageDetailPage/>}/>
                        <Route path="problems" element={<ProblemPage />} />
                        <Route path="problems/:id" element={<ProblemDetailPage/>}/>
                        <Route path="problems/add" element={<ProblemAddPage/>}/>
                        <Route path="problem-templates" element={<ProblemTemplatePage />} />
                        <Route path="problem-templates/:id" element={<ProblemTemplateDetailPage />}/>
                        <Route path="problem-templates/add" element={<ProblemTemplateAddPage/>}/>
                        <Route path="tags" element={<TagPage/>}/>
                        <Route path="tags/add" element={<TagAddPage/>}/>
                        <Route path="tags/:id" element={<TagDetailPage/>}/>
                    </Route>
                </Route>
                <Route path="/admin/auth/login" element={<AdminLoginPage/>}/>
            </Routes>
        </BrowserRouter>
    );
}
