export interface ProblemTemplateAddRequest {
    langId: string;
    templateCode: string;
    runnerCode: string;
    compileCommand?: string;
    runCommand?: string;
}