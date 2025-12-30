import axios from "axios";
import { enableAdminOnlyResponseInterceptor, enableApiProblemDetailParsingInterceptor, enableUserAuthInterceptor } from "./interceptors";

const domain = "localhost:7254";

export const authApi = axios.create({
    baseURL: "https://" + domain + "/auth",
    withCredentials: true,
    headers: {"Content-Type": "application/json"},
});

export const profileApi = axios.create({
    baseURL: "https://" + domain + "/profile",
    withCredentials: true,
    headers: {"Content-Type": "application/json"},
});

export const submitApi = axios.create({
    baseURL: "https://" + domain + "/submit",
    withCredentials: true,
    headers: {"Content-Type": "application/json"},
});

export const problemApi = axios.create({
    baseURL: "https://" + domain + "/problems",
    withCredentials: true,
    headers: {"Content-Type": "application/json"},
});

/* Admin endpoints */
export const adminMiscApi = axios.create({
    baseURL: "https://" + domain + "/admin",
    withCredentials: true,
    headers: {"Content-Type": "application/json"},
});

export const adminAuthApi = axios.create({
    baseURL: "https://" + domain + "/admin/auth",
    withCredentials: true,
    headers: {"Content-Type": "application/json"},
});

/* Domain endpoints */
export const adminProblemApi = axios.create({
    baseURL: "https://" + domain + "/admin/problems",
    withCredentials: true,
    headers: {"Content-Type": "application/json"},
});

export const adminLanguageApi = axios.create({
    baseURL: "https://" + domain + "/admin/languages",
    withCredentials: true,
    headers: {"Content-Type": "application/json"},
});

export const adminProblemTemplateApi = axios.create({
    baseURL: "https://" + domain + "/admin/templates",
    withCredentials: true,
    headers: {"Content-Type": "application/json"},
});

export const adminTagApi = axios.create({
    baseURL: "https://" + domain + "/admin/tags",
    withCredentials: true,
    headers: {"Content-Type": "application/json"},
});

export const adminUserApi = axios.create({
    baseURL: "https://" + domain + "/admin/users",
    withCredentials: true,
    headers: {"Content-Type": "application/json"},
});


/* Interceptor enable for user auth */
enableUserAuthInterceptor(authApi);
enableUserAuthInterceptor(profileApi);

/* Interceptor to enable status code map to redirect page */
enableAdminOnlyResponseInterceptor(adminMiscApi);

/* Interceptor enable for api problem detail parsing */
enableApiProblemDetailParsingInterceptor(authApi);
enableApiProblemDetailParsingInterceptor(adminProblemApi);
enableApiProblemDetailParsingInterceptor(profileApi);
enableApiProblemDetailParsingInterceptor(submitApi);
enableApiProblemDetailParsingInterceptor(adminLanguageApi);
enableApiProblemDetailParsingInterceptor(adminProblemTemplateApi);
enableApiProblemDetailParsingInterceptor(adminTagApi);
enableApiProblemDetailParsingInterceptor(adminMiscApi);
enableApiProblemDetailParsingInterceptor(adminAuthApi);

