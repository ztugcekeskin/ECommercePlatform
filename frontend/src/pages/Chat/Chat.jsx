import { useEffect, useRef, useState } from "react";
import axios from "axios";
import "./Chat.css";

function Chat() {
    const [messages, setMessages] = useState([]);
    const [selectedUser, setSelectedUser] = useState(null);
    const [message, setMessage] = useState("");
    const [conversationMessages, setConversationMessages] = useState([]);
    const userId = localStorage.getItem("userId");
    const socketRef = useRef(null);
    const selectedUserRef = useRef(null);

    useEffect(() => {
    selectedUserRef.current = selectedUser;
}, [selectedUser]);
    
    useEffect(() => {
    if (!userId) {
        console.log("WebSocket: userId bulunamadı.");
        return;
    }

    console.log("WebSocket bağlantısı deneniyor:", userId);

    const socket = new WebSocket(
        `ws://localhost:5070/ws/chat/${userId}`
    );

    socketRef.current = socket;

    socket.onopen = () => {
        console.log("✅ WebSocket bağlantısı kuruldu!");
    };

    socket.onmessage = (event) => {

    try {
        const newMessage = JSON.parse(event.data);
        console.log(
            "📩 WebSocket mesajı:",
            newMessage
        );

        setMessages((prev) => {
            if (
                newMessage.id &&
                prev.some(
                    (m) => m.id === newMessage.id
                )
            ) {
                return prev;
            }
            return [...prev, newMessage];
        });

        const currentConversation =
            selectedUserRef.current;

        if (
            currentConversation &&
            Number(newMessage.productId) ===
                Number(currentConversation.productId) &&
            (
                Number(newMessage.senderId) ===
                    Number(currentConversation.userId) ||
                Number(newMessage.receiverId) ===
                    Number(currentConversation.userId)
            )
        ) {

            setConversationMessages((prev) => {
                if (
                    newMessage.id &&
                    prev.some(
                        (m) => m.id === newMessage.id
                    )
                ) {
                    return prev;
                }
                return [
                    ...prev,
                    newMessage
                ];
            });
        }
    } catch (error) {
        console.error(
            "WebSocket mesajı okunamadı:",
            error
        );
    }
};
    socket.onerror = (error) => {
        console.error("❌ WebSocket hatası:", error);
    };

    socket.onclose = (event) => {
        console.log("🔌 WebSocket bağlantısı kapandı:", event);
    };

    return () => {
        socket.close();
        socketRef.current = null;
    };
}, [userId]);

    useEffect(() => {

    if (!userId) return;

    const getMessages = async () => {

        try {

            const response = await axios.get(
                `http://localhost:5070/api/Chat/user/${userId}`
            );

            console.log("Chat messages:", response.data);

            setMessages(response.data);

        } catch (error) {

            console.error("Mesajlar alınamadı:", error);

        }
    };

    getMessages();

}, [userId]);

    useEffect(() => {

    if (!selectedUser) return;

    const getConversationMessages = async () => {

        try {

            const response = await axios.get(
                `http://localhost:5070/api/Chat?userId=${userId}&otherUserId=${selectedUser.userId}&productId=${selectedUser.productId}`
            );

            setConversationMessages(response.data);

        } catch (error) {

            console.error("Konuşma alınamadı:", error);

        }
    };

    getConversationMessages();

}, [selectedUser, userId]);


    // Müşterileri grupla
    const conversations = Object.values(
    messages.reduce((groups, chatMessage) => {

        const currentUserId = Number(userId);

        let otherUserId;

        if (Number(chatMessage.senderId) === currentUserId) {
            otherUserId = Number(chatMessage.receiverId);
        } else {
            otherUserId = Number(chatMessage.senderId);
        }

        const productId = Number(chatMessage.productId);

        // Aynı müşteri + aynı ürün = tek konuşma
        const key = `${otherUserId}-${productId}`;

        if (!groups[key]) {
            groups[key] = {
                userId: otherUserId,
                productId: productId,
                messages: []
            };
        }

        groups[key].messages.push(chatMessage);

        return groups;

    }, {})
);

    const sendMessage = async () => {

    if (!message.trim() || !selectedUser) {
        return;
    }

    const newMessage = {
        senderId: Number(userId),
        receiverId: Number(selectedUser.userId),
        productId: Number(selectedUser.productId),
        message: message.trim()
    };

    try {

        await axios.post(
            "http://localhost:5070/api/Chat",
            newMessage
        );

        setMessage("");

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