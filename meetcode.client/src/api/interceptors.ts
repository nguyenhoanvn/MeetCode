import { ApiProblemDetail } from "../types/system/apiProblemDetail";
import {refresh} from "./auth";
import { AxiosInstance, AxiosError, InternalAxiosRequestConfig } from "axios";

export const enableUserAuthInterceptor = (axiosInstance: AxiosInstance): void => {
    axiosInstance.interceptors.response.use(
        (response) => response,
        async (error: AxiosError) => {
            const request = error.config as InternalAxiosRequestConfig & {_retry: boolean};

            if (error.response?.status === 401 && !request._retry) {
                request._retry = true;
                try {
                    await refresh();
                    return axiosInstance(request);
                } catch (refreshError) {
                    return Promise.reject(refreshError);
                }
            }
            return Promise.reject(error);
        }
    )
}

export const enableApiProblemDetailParsingInterceptor = (axiosInstance: AxiosInstance): void => {
    axiosInstance.interceptors.response.use(
        res => res,
        (error: AxiosError<ApiProblemDetail>) => {
            return Promise.reject(error.response?.data);
        }
    )
}