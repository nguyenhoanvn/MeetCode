import { useEffect, useRef, useState } from "react";
import { useNavigate } from "react-router-dom";

type UserMenuProps = {
    handleLogout: () => Promise<void> | void;
};

export default function UserMenu({ handleLogout }: UserMenuProps) {
    const [open, setOpen] = useState<boolean>(false);
    const menuRef = useRef<HTMLDivElement | null>(null);
    const navigate = useNavigate();

    useEffect(() => {
        const handleClickOutside = (event: MouseEvent) => {
            if (
                menuRef.current &&
                !menuRef.current.contains(event.target as Node)
            ) {
                setOpen(false);
            }
        };

        document.addEventListener("mousedown", handleClickOutside);
        return () => {
            document.removeEventListener("mousedown", handleClickOutside);
        };
    }, []);

    const onLogoutClick = async (): Promise<void> => {
        await handleLogout();
        setOpen(false);
        window.location.reload();
    };

    return (
        <div className="ml-auto mr-10 relative" ref={menuRef}>
            {/* Avatar */}
            <span
                className="material-symbols-outlined text-4xl cursor-pointer text-gray-200 text-4xl!"
                onClick={() => setOpen((prev) => !prev)}
            >
                account_circle
            </span>

            {/* Dropdown */}
            {open && (
                <div className="absolute right-0 mt-2 w-40 bg-gray-800 rounded-md shadow-lg border border-gray-700 z-50">
                    <button
                        type="button"
                        onClick={onLogoutClick}
                        className="w-full text-left px-4 py-2 text-sm text-gray-200 hover:bg-[#1E3A8A]"
                    >
                        Logout
                    </button>
                </div>
            )}
        </div>
    );
}