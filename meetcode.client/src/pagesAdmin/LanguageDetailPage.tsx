import { useParams } from "react-router-dom";
import useLanguageDetail from "../hooksAdmin/useLanguageDetail";

export default function LanguageDetailPage() {
    const { id } = useParams<{ id: string}>();
    const { language, loading, error } = useLanguageDetail(id!);

    return (
        <div>
            <p>Hello {language?.name}</p>
        </div>
    )
}