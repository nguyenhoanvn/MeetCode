import TagListResponse from "../../types/response/tag/tagListResponse"
import TagResponse from "../../types/response/tag/tagResponse";
import { adminTagApi } from "../client"

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