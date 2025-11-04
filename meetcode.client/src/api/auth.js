import authApi from "./client";

export const register = (loginRequest) => authApi.post("/register", loginRequest);

export const login = () => authApi.get("/login");
