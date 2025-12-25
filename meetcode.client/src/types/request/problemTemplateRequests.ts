export interface ProblemTemplateAddRequest {
    problemId: string;
    langId: string;
    methodName: string;
    returnType: string;
    parameters: string[];
    compileCommand: string | null;
    runCommand: string | null;
}