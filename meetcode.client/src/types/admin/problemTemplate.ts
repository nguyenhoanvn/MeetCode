import { Language } from "./language";

export interface ProblemTemplate {
    templateId: string;
    problemId: string;
    langId: string;
    templateCode: string;
    runnerCode: string;
    compileCommand: string | null;
    runCommand: string | null;
    isEnabled: boolean;
    problem: Problem;
    language: Language;
}