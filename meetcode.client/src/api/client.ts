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

export const problemApi = axios.create({
    baseURL: "https://" + domain + "/problems",
    withCredentials: true,
    headers: {"Content-Type": "application/json"},
});

export const submitApi = axios.create({
    baseURL: "https://" + domain + "/submit",
    withCredentials: true,
    headers: {"Content-Type": "application/json"},
})

enableUserAuthInterceptor(authApi);
enableUserAuthInterceptor(profileApi);

enableApiProblemDetailParsingInterceptor(authApi);
enableApiProblemDetailParsingInterceptor(problemApi);
enableApiProblemDetailParsingInterceptor(profileApi);
enableApiProblemDetailParsingInterceptor(submitApi);