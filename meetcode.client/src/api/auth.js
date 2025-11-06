import authApi from "./client";

export const register = async (registerRequest) => await authApi.post("/register", registerRequest);

export const login = async (loginRequest) =>  {
    const response = await authApi.post("/login", loginRequest);
    return response.data;
}
