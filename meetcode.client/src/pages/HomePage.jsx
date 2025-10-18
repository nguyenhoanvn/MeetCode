export default function HomePage() {
    return (
        <div className="p-6 text-center">
            <h1 className="text-3x1 font-bold mb-4">Welcome</h1>
            <p className="text-gray-700">
                Hello this is main page.
                <a href="/login">Login</a>
                and <a href="/register">Register</a>
            </p>
        </div>
    );
}