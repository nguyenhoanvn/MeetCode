import { AuditLog } from "./auditLog";
import { Problem } from "./problem";
import { Submission } from "./submission";

export type Role = 'admin' | 'moderator' | 'user';

export interface User {
    userId: string;
    email: string;
    displayName: string;
    role: Role;
    isActive: boolean;
    createdAt: string;
    lastLoginAt: string;
    authProvider: string;
    auditLogs: AuditLog[];
    problems: Problem[];
    submissions: Submission[];
}