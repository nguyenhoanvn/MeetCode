export interface LanguageUpdateRequest {
    name?: string;
    version?: string;
    fileExtension?: string;
    compileImage?: string;
    runtimeImage?: string;
    compileCommand?: string | null;
    runCommand?: string | null;
}