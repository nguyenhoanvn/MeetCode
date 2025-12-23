interface LoadingOverlayProps {
    message: string;
};

export default function LoadingOverlay({ message } : LoadingOverlayProps) {
    return (
        <div className="fixed inset-0 z-50 flex items-center justify-center bg-black/60 backdrop-blur-sm">
            <div className="flex flex-col items-center gap-4">
                <span className="material-symbols-outlined animate-spin">
                    progress_activity
                </span>
                <p className="text-white text-lg font-light">{message}</p>
            </div>
        </div>
    )
}