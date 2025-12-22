import { useParams } from "react-router-dom";
import useProblemTemplateDetail from "../hooksAdmin/useProblemTemplateDetail";

export default function ProblemTemplateDetailPage() {
    const { id } = useParams<{ id: string }>();
    const { problemTemplate, error, loading } = useProblemTemplateDetail(id!);

    return (
        <div>
            {problemTemplate?.templateId}
        </div>
    );
}