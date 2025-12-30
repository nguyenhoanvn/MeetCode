import { Language } from "./language";
import { Problem } from "./problem";
import { User } from "./user";

export interface Submission {
    submissionId: string;
    userId: string;
    problemId: string;
    langId: string;
    verdict: string;
    sourceCode: string;
    execTimeMs: number | null;
    memoryKb: number | null;
    lang: Language;
    problem: Problem;
    user: User;
}