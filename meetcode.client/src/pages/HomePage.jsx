export default function HomePage() {
    return (
        <table className="p-6 text-center">
            <tr>
                <th>Item</th>
                <td>Description</td>
                <td>Link</td>
            </tr>
            <tr>
                <th>Register</th>
                <td>Allow user to register new account</td>
                <td><a href="/auth/register">Register</a></td>
            </tr>
        </table>
    );
}