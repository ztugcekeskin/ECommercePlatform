import { useEffect, useState } from "react";
import axios from "axios";
import "./Chat.css";

function Chat() {
    const [messages, setMessages] = useState([]);
    const [selectedUser, setSelectedUser] = useState(null);
    const [message, setMessage] = useState("");
    const [conversationMessages, setConversationMessages] = useState([]);
    const userId = localStorage.getItem("userId");

    useEffect(() => {
        if (!userId) return
        axios
            .get(`http://localhost:5070/api/Chat/user/${userId}`)
            .then((response) => {
                console.log("Chat messages:", response.data);
                setMessages(response.data);
            })
            .catch((error) => {
                console.error("Mesajlar alınamadı:", error);
            });
}, [userId]);

    // Müşterileri grupla
    const conversations = Object.values(
        messages.reduce((groups, message) => {

        const otherUserId =
        message.senderId === Number(userId)
        ? message.receiverId
        : message.senderId;

        const key = `${otherUserId}-${message.productId}`;
            if (!groups[key]) {
            groups[key] = {
            userId: otherUserId,
            productId: message.productId,
            messages: []
            };
            }
            groups[key].messages.push(message);
            return groups;
        }, {})
    )

    const sendMessage = async () => {

    if (!message.trim() || !selectedUser) {
        return;
    }
    try {
        await axios.post(
            "http://localhost:5070/api/Chat",
            {
                senderId: Number(userId),
                receiverId: selectedUser.userId,
                productId: selectedUser.productId,
                message: message
            }
        );

        setMessage("");
        // Konuşmayı tekrar getiriyor
        const response = await axios.get(
            `http://localhost:5070/api/Chat?userId=${userId}&otherUserId=${selectedUser.userId}&productId=${selectedUser.productId}`
        );
        setConversationMessages(response.data);
    } catch (error) {
        console.error("Mesaj gönderilemedi:", error);
    }
};

    return (
        <div className="chat-page">
            <h2>Mesajlar</h2>
            {conversations.length === 0 ? (
            <p className="no-messages">
                Henüz mesajınız bulunmuyor.
            </p>
            ) : (
            <div className="conversation-list">
            {conversations.map((conversation) => {
            const lastMessage =
            conversation.messages[
            conversation.messages.length - 1
];
    return (
         <div
            className="conversation-card"
            key={`${conversation.userId}-${conversation.productId}`}
            onClick={async () => {
            setSelectedUser(conversation);
            try {
        const response = await axios.get(
            `http://localhost:5070/api/Chat?userId=${userId}&otherUserId=${conversation.userId}&productId=${conversation.productId}`
        );

        setConversationMessages(response.data);
        } catch (error) {
        console.error("Konuşma alınamadı:", error);
        }
        }}
        >
        <div className="conversation-info">
            <h3>
            Müşteri #{conversation.userId}
            </h3>

            <p>
            {lastMessage.message}
             </p>

            <small>
            {new Date(
            lastMessage.createdAt).toLocaleString("tr-TR")}
            </small>
            </div>
                    
            <div className="conversation-product">
            <span>
            Ürün ID
            </span>

            <strong>
            #{conversation.productId}
            </strong>
        </div>
    </div>
);})}
</div>
)}
        {selectedUser && (
        <div className="seller-chat-box">
        <div className="seller-chat-header">

            <h3>Müşteri #{selectedUser.userId}</h3>
            <button
                onClick={() => {
                setSelectedUser(null);
                setConversationMessages([]);
                }}
            >
            ✕
            </button>
        </div>

        <div className="seller-chat-messages">
            {conversationMessages.map((chat) => (
                <div
                    key={chat.id}
                    className={
                    chat.senderId === Number(userId)
                    ? "seller-my-message"
                    : "seller-customer-message"
                    }
                >
            {chat.message}
        </div>
    ))}
</div>

        <div className="seller-chat-input">
            <input
            type="text"
            placeholder="Cevap yaz..."
            value={message}
            onChange={(e) => setMessage(e.target.value)}
            onKeyDown={(e) => {
            if (e.key === "Enter") {
                sendMessage();
                }
            }}
        />
            <button onClick={sendMessage}>
            ➤
            </button>
        </div>
    </div>
)})
</div>
);}
export default Chat;