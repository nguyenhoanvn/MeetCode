import { Language } from "./language";
import { Problem } from "./problem";

export interface Submission {
    submissionId: string;
    userId: string;
    language: Language;
    problem: Problem;
    verdict: string;
    execTimeMs: number;
}