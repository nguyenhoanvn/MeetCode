import { TagAddRequest } from "../../types/request/tagRequests";
import { adminTagApi } from "../client"
import { TagListResponse, TagResponse } from "../../types/response/tagResponses";

/* Get all tag endpoint */
export const tagList = async () => {
    const response = await adminTagApi.get<TagListResponse>("");
    return response.data.tagList;
}

/* Get tag by id endpoint */
export const tagGet = async (id: string) => {
    const response = await adminTagApi.get<TagResponse>(`/${id}`);
    return response.data.tag;
}

/* Search tag containing name endpoint */
export const tagSearch = async (name: string) => {
    const response = await adminTagApi.get<TagListResponse>(`/search?name=${name}`);
    return response.data.tagList;
}

/* Tag add endpoint */
export const tagAdd = async (request: TagAddRequest) => {
    const response = await adminTagApi.post<TagResponse>("", request);
    console.log(response);
    return response.data.tag;
}