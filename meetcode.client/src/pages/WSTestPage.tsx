import { useEffect, useState } from "react";

export function WSTestPage() {
    const [ws, setWs] = useState<WebSocket | null>(null);
    const [message, setMessage] = useState("");
    const [messages, setMessages] = useState<string[]>([]);

    const sendJob = () => {
        if (ws) {
            const job = {
                JobId: "3219fc89-19fc-467f-9e4e-225559f7a5c7",
                Status: "queued",
                MessageSent: {
                    Code: `public class Solution
    {
        public int TwoSum(int a, int b)
        {
            return a + b;
        }
    }`,
                    JobId: "3219fc89-19fc-467f-9e4e-225559f7a5c7",
                    LanguageName: "csharp",
                    ProblemId: "50b25965-97ce-4aa8-8aca-a0c44c9e2e30",
                    TestCaseIds: ["77fdcfa9-513c-4023-a64b-f4da771b2180"]
                },
                QueueName: "run_code_queue"
            };

            ws.send(JSON.stringify(job));
            setMessages(prev => [...prev, `Sent job ${job.JobId}`]);
        }
    };

    useEffect(() => {
        const socket = new WebSocket("ws://localhost:8181");
        setWs(socket);

        socket.onopen = () => {
            console.log("Connected to WebSocket server");
        };

        socket.onmessage = (event) => {
            console.log("Received from server:", event.data);
            setMessages(prev => [...prev, `Server: ${event.data}`]);
        };

        return () => {
            socket.close();
        };
    }, []);

    const sendMessage = () => {
        if (ws && message) {
            ws.send(message);
            setMessages(prev => [...prev, `You: ${message}`]);
            setMessage("");
        }
    };

    return (
        <div>
            <h1>WebSocket Test Page</h1>
            <div>
                <input
                    type="text"
                    value={message}
                    onChange={(e) => setMessage(e.target.value)}
                    placeholder="Type a message"
                />
                <button onClick={sendJob}>Send</button>
            </div>
            <div>
                {messages.map((msg, idx) => (
                    <p key={idx}>{msg}</p>
                ))}
            </div>
        </div>
    );
}
