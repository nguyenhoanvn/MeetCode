import { format } from "date-fns";

export function formatDate(iso?: string | null): string {
    if (!iso) return "unknown";

    const safeIso = iso.replace(/\.(\d{3})\d+/, '.$1');
    const date = new Date(safeIso);

    if (isNaN(date.getTime())) return "invalid date";

    return format(date, "PPP p");
}
