import { useNavigate } from "react-router-dom";
import { isAdmin } from "../helpers/adminCheck";
import { ApiProblemDetail } from "../types/system/apiProblemDetail";
import {refresh} from "./auth";
import axios, { AxiosInstance, AxiosError, InternalAxiosRequestConfig } from "axios";

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
                    window.location.href = "/auth/login";
                    return Promise.reject(refreshError);
                }
            }
            return Promise.reject(error);
        }
    )
}

export const enableAdminOnlyResponseInterceptor = (
    axiosInstance: AxiosInstance
): void => {
    axiosInstance.interceptors.response.use(
        res => res,
        async (error: AxiosError) => {
            const status = error.response?.status;

            if (status === 401) {
                window.location.href = "admin/auth/login";
            }

            if (status === 403) {
                window.location.href = "/forbidden";
            }

            return Promise.reject(error);
        }
    );
};

export const enableApiProblemDetailParsingInterceptor = (axiosInstance: AxiosInstance): void => {
    axiosInstance.interceptors.response.use(
        res => res,
        (error: AxiosError<ApiProblemDetail>) => {
            return Promise.reject(error.response?.data);
        }
    )
}