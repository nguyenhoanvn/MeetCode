import { User } from "./user";

export interface AuditLog {
    auditId: string;
    actorUserId: string;
    action: string;
    entity: string;
    entityId: string;
    timeStamp: string;
    metadataJson: string;
    user: User;
}