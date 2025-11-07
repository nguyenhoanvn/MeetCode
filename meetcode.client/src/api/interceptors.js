import {refresh} from "./auth";

export const enableUserAuthInterceptor = (axiosInstance) => {
    axiosInstance.interceptors.response.use(
        response => response,
        async (error) => {
            const request = error.config;

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