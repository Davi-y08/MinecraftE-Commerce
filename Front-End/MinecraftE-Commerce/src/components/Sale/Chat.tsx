import { useEffect, useState, useRef } from "react";
import * as signalR from "@microsoft/signalr";
import { useParams } from "react-router-dom";

function Chat() {
    interface ChatMessage {
        chatId: number;
        text: string;
        senderId: string;
        sentAt: string;
    }

    const { chatId, idUser } = useParams();
    const [messages, setMessages] = useState<ChatMessage[]>([]);
    const [messageText, setMessageText] = useState('');
    const connectionRef = useRef<signalR.HubConnection | null>(null);

    useEffect(() => {
        const connect = new signalR.HubConnectionBuilder()
            .withUrl("https://localhost:7253/chat", {
                accessTokenFactory: () => localStorage.getItem("token") || ""
            })
            .withAutomaticReconnect()
            .build();

        connectionRef.current = connect;

        connect.on("ReceiveMessage", (message: ChatMessage) => {
            setMessages(prev => [...prev, message]);
        });

        connect.start()
            .then(() => console.log("Conectado ao chat!"))
            .catch(err => console.error("Erro ao conectar:", err));

        return () => {
            connect.stop();
        };
    }, []);

    const sendMessage = async () => {
        if (!connectionRef.current || !chatId) return;

        try {
            await connectionRef.current.invoke('SendMessage', parseInt(chatId), messageText);
            setMessageText('');
        } catch (error) {
            console.error("Erro ao enviar a mensagem! ", error);
        }
    };

    return (
        <div>
            <div>
                <h2>Chat ID: {chatId}</h2>
                <ul>
                    {messages.map((msg, index) => (
                        <li key={index}>
                            <b>{msg.senderId}</b>: {msg.text} ({new Date(msg.sentAt).toLocaleTimeString()})
                        </li>
                    ))}
                </ul>
            </div>

            <input
                type="text"
                value={messageText}
                onChange={(e) => setMessageText(e.target.value)}
                placeholder="Digite sua mensagem"
            />
            <button onClick={sendMessage}>Enviar</button>
        </div>
    );
}

export default Chat;
