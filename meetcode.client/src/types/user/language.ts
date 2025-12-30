export interface Language {
    langId: string;
    name: string;
    version: string;
    runTimeImage: string;
    compileCommand?: string;
    runCommand?: string;
    isEnabled: boolean;
}