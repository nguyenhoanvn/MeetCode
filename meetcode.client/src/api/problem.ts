import { problemApi } from "./client"

export const problemList = async () => {
    const response = await problemApi.get("");
    return response.data;
} 

export const problemSearch = async (name: string) => {
    const response = await problemApi.get(`/search?name=${name}`);
    return response.data;
}

export const problemDetail = async (slug: string) => {
    const response = await problemApi.get(`${slug}`);
    return response.data;
}