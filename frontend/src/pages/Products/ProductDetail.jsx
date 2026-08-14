import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import "./ProductDetail.css";
import axios from "axios";

function ProductDetail() {
  const { id } = useParams();

  const [product, setProduct] = useState(null);
  const [reviews, setReviews] = useState([]);
  const [chatOpen, setChatOpen] = useState(false);
  const [message, setMessage] = useState("");
  const [chatMessages, setChatMessages] = useState([]);

  useEffect(() => {
    // PostgreSQL için
    axios
      .get(`http://localhost:5070/api/Product/${id}`)
      .then((response) => {
        setProduct(response.data);
      })
      .catch((error) => {
        console.error("Ürün alınamadı:", error);
      });

    // MongoDBdeki için
    axios
      .get(`http://localhost:5070/api/Review/product/${id}`)
      .then((response) => {
        console.log("Reviews:", response.data);
        setReviews(response.data);
      })
      .catch((error) => {
        console.error("Yorumlar alınamadı:", error);
      });
  }, [id]);

  useEffect(() => {

    if (!product) return;

    const customerId = localStorage.getItem("userId");

    axios
        .get(
            `http://localhost:5070/api/Chat?userId=${customerId}&otherUserId=${product.seller.id}&productId=${id}`
        )
        .then((response) => {
            console.log("Chat messages:", response.data);
            setChatMessages(response.data);
        })
        .catch((error) => {
            console.error("Chat mesajları alınamadı:", error);
        });

}, [product, id]);

  const averageRating = reviews.length > 0 ? reviews.reduce(
      (total, review) => total + review.rating, 0) / reviews.length : 0;

    const sendMessage = async () => {

    if (!message.trim()) {
        return;
    }

    const customerId = localStorage.getItem("userId");

    try {
    await axios.post("http://localhost:5070/api/Chat",
        {
          senderId: Number(customerId),
          receiverId: product.seller.id,
          productId: Number(id),
          message: message
        }
      );

      setMessage("");
        // Mesaj gönderildikten sonra konuşmayı tekrar getir
      const response = await axios.get(`http://localhost:5070/api/Chat?userId=${customerId}&otherUserId=${product.seller.id}&productId=${id}`);
        setChatMessages(response.data);

    } catch (error) {
        console.error("Mesaj gönderilemedi:", error);
    }
  };

  if (!product) {
    return <p>Ürün yükleniyor...</p>;
  }

  return (
    <div className="product-detail-page">

      {/* Ürün bilgileri */}
      <div className="product-detail-card">

        <img
          className="product-detail-image"
          src={
            product.imageUrl
              ? "http://localhost:5070" + product.imageUrl
              : "https://placehold.co/500x400?text=Ürün"
          }
          alt={product.name}
        />

        <div className="product-detail-info">

          <h1>{product.name}</h1>

          <div className="product-rating">

          <span className="stars">
          {reviews.length > 0
          ? "★".repeat(Math.round(averageRating))
          : "☆☆☆☆☆"}
          </span>

          {reviews.length > 0 && (
          <span className="average-rating">
            {averageRating.toFixed(1)}
          </span>
          )}

          <span className="review-count">
          ({reviews.length} yorum)
          </span>
          </div>

          <div className="seller-info">
            <strong>Satıcı:</strong>{" "}
            {product.seller?.username || "Satıcı"}
          </div>

          <p className="product-detail-price">
            {product.price} TL
          </p>

          <p className="product-detail-stock">
            Stok: {product.stock}
          </p>

          <button
            className="detail-cart-btn"
            disabled={product.stock === 0}
          >
            {product.stock === 0
              ? "Stokta Yok"
              : "🛒 Sepete Ekle"}
          </button>

        </div>
      </div>

      {/* Açıklama */}
      <div className="product-extra">

        <h2>Ürün Açıklaması</h2>

        <p className="product-description">
          {product.description}
        </p>

      </div>

      {/* Yorumlar */}
      <div className="product-extra">

        <h2>Değerlendirmeler</h2>

        {reviews.length === 0 ? (
          <p className="product-description">
            Henüz bu ürün için yorum yapılmamış.
          </p>
          ) : (
          <div className="reviews-list">

            {reviews.map((review) => (
              <div className="review-card" key={review.id}>
                <div className="review-header">

                <strong>
                  {review.userName || "Müşteri"}
                </strong>

                <span className="review-stars">
                  {"★".repeat(review.rating)}
                  {"☆".repeat(5 - review.rating)}
                </span>

                </div>

                <p className="review-comment">
                  {review.comment}
                </p>

                <small className="review-date">
                  {new Date(review.createdAt).toLocaleDateString("tr-TR")}
                </small>

              </div>
            ))}
          </div>
        )}
      </div>

        {/* Chat */}

<div className="chat-container">

    {!chatOpen && (
        <button
            className="chat-open-btn"
            onClick={() => setChatOpen(true)}
        >
            💬 Satıcıya Sor
        </button>
    )}

    {chatOpen && (
        <div className="chat-box">
          <div className="chat-header">
            <strong> 💬 {product.seller?.username || "Satıcı"} </strong>
            <button
              onClick={() => setChatOpen(false)}
            >
                ✕
            </button>
            </div>

            <div className="chat-messages">
            {chatMessages.length === 0 ? (
              <p>Henüz mesaj yok.</p>
              ) : (
              chatMessages.map((chat) => (
                <div
                key={chat.id}
                className={
                chat.senderId === Number(localStorage.getItem("userId"))
                ? "chat-message my-message"
                : "chat-message seller-message"
                }
              >
                {chat.message}
                </div>
                ))
              )}
            </div>
            <div className="chat-input">
            <input
            type="text"
            placeholder="Mesaj yaz..."
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
    )}
  </div>
</div>);
} export default ProductDetail;