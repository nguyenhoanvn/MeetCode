export type ApiProblemDetail = {
    title: string;
    status: number;
    detail?: string;
    errors?: Record<string, string[]>;
};