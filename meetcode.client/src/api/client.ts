import axios from "axios";
import { enableApiProblemDetailParsingInterceptor, enableUserAuthInterceptor } from "./interceptors";

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

/* Domain endpoints */
export const problemApi = axios.create({
    baseURL: "https://" + domain + "/problems",
    withCredentials: true,
    headers: {"Content-Type": "application/json"},
});

export const languageApi = axios.create({
    baseURL: "https://" + domain + "/admin/languages",
    withCredentials: true,
    headers: {"Content-Type": "application/json"},
});

export const problemTemplateApi = axios.create({
    baseURL: "https://" + domain + "/admin/templates",
    withCredentials: true,
    headers: {"Content-Type": "application/json"},
});


/* Interceptor enable for user auth */
enableUserAuthInterceptor(authApi);
enableUserAuthInterceptor(profileApi);

/* Interceptor enable for api problem detail parsing */
enableApiProblemDetailParsingInterceptor(authApi);
enableApiProblemDetailParsingInterceptor(problemApi);
enableApiProblemDetailParsingInterceptor(profileApi);
enableApiProblemDetailParsingInterceptor(submitApi);
enableApiProblemDetailParsingInterceptor(languageApi);
enableApiProblemDetailParsingInterceptor(problemTemplateApi);