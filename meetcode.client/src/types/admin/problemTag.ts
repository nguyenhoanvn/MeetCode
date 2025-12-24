import { Problem } from "./problem";

export interface ProblemTag {
    tagId: string;
    name: string;
    problems: Problem[];
}