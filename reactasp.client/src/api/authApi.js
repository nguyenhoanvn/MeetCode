import axios from "axios";

const api = axios.create({
    baseURL: "https://localhost:7254",
    withCredentials: true,
    headers: {
        "Content-Type": "application/json"
    }
});

export const login = async (email, password) => {
    
    const response = await api.post("/auth/login", { email, password });
    return response.data; 
};

export const register = async (email, password, displayName) => {
    const response = await api.post("/auth/register", { email, password, displayName });
    return response.data;
}