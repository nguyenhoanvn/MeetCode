import { ProblemTemplate } from "./problemTemplate";
import { Submission } from "./submission";

export interface Language {
    langId: string;
    name: string;
    version: string;
    fileExtension: string;
    compileImage: string;
    runtimeImage: string;
    compileCommand: string | null;
    runCommand: string | null;
    isEnabled: boolean;
    submissions: Array<Submission>;
    problemTemplates: Array<ProblemTemplate>;
}