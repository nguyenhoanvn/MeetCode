import axios from "axios";

const authApi = axios.create({
    baseURL: "https://localhost:7254/auth",
    headers: {"Content-Type": "application/json"},
});

export default authApi;