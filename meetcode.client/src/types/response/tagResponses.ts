import { ProblemTag } from "../admin/problemTag";

export interface TagResponse {
    tag: ProblemTag;
}

export interface TagListResponse {
    tagList: ProblemTag[];
}