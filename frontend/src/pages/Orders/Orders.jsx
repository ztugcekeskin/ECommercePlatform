import { useEffect, useState } from "react";
import axios from "axios";
import "./Orders.css";

function Orders() {

    const [orders, setOrders] = useState([]);
    const userId = localStorage.getItem("userId");
    const role = localStorage.getItem("role");
    const [showReviewForm, setShowReviewForm] = useState(false);
    const [selectedProduct, setSelectedProduct] = useState(null);
    const [rating, setRating] = useState(5);
    const [comment, setComment] = useState("");
    console.log("Seller userId:", userId);
    console.log("Role:", role);

    useEffect(() => {

    const url =
        role === "Seller"
            ? `http://localhost:5070/api/Order/seller/${userId}`
            : `http://localhost:5070/api/Order/${userId}`;

    axios
        .get(url)
        .then((response) => {
            setOrders(response.data);
        })
        .catch((error) => {
            console.error(error);
        });

},[role, userId]);

    if (role === "Customer") {

    return (
    <div className="orders-container">
        <h2>Siparişlerim</h2>
        {orders.length === 0 ? (
            <p>Henüz siparişiniz bulunmuyor.</p>
            ) : (
            orders.map(order => (
            <div
                className="order-card"
                key={order.id}
            >

            <h3>
            Sipariş #{order.id}
            </h3>

            <p>
            Tarih:
            {" "}
            {new Date(order.orderDate).toLocaleString()}
            </p>

            <p>
            Durum:
            <span className={`status ${
            order.status === "Tamamlandı"
            ? "completed"
            : order.status === "İptal Edildi"
            ? "cancelled"
            : "preparing"
            }`}>
            {order.status}
            </span>
            </p>

            <hr />

            {order.orderItems.map(item => (

            <div
            className="order-item"
            key={item.id}
            >

            <img
            src={
            item.product.imageUrl
            ? "http://localhost:5070" + item.product.imageUrl
            : "https://placehold.co/100x100?text=Ürün"
            }
                alt={item.product.name}
            />
            <div>

            <h4>{item.product.name}</h4>

            <p>
            Adet: {item.quantity}
            </p>

            <p>
            Birim Fiyat:
            {" "}
            {item.unitPrice} TL
            </p>

            {order.status === "Tamamlandı" && (
            <button
            className="review-btn"
            onClick={() => {
            setSelectedProduct(item.product);
            setRating(5);
            setComment("");
            setShowReviewForm(true);
            }}
            >
            ⭐ Yorum Yap
            </button>
        )}

            </div>
            </div>
     ))}

    <h3 className="total-price">
    Toplam:
    {" "}
    {order.totalPrice} TL
    </h3>{showReviewForm && selectedProduct && (
    <div className="review-form">

        <h3>
            {selectedProduct.name} - Değerlendir
        </h3>

        <div className="rating-select">

            {[1, 2, 3, 4, 5].map((star) => (
                <button
                    key={star}
                    type="button"
                    className={
                        star <= rating
                            ? "star active"
                            : "star"
                    }
                    onClick={() => setRating(star)}
                >
                    ★
                </button>
            ))}

        </div>

        <textarea
            value={comment}
            onChange={(e) => setComment(e.target.value)}
            placeholder="Ürün hakkında ne düşünüyorsunuz?"
        />

        <div className="review-form-buttons">
            <button
            className="submit-review-btn"
            onClick={async () => {
            if (!comment.trim()) {
                alert("Lütfen yorum yazın.");
                return;
          }
            try {
                const userId =localStorage.getItem("userId");
                const userName =localStorage.getItem("username");

                await axios.post("http://localhost:5070/api/Review",
                    {
                        productId: selectedProduct.id,
                        userId: Number(userId),
                        userName: userName || "Müşteri",
                        rating: rating,
                        comment: comment
                    }
            );
            alert("Yorumunuz başarıyla gönderildi.");
                        setShowReviewForm(false);
                        setSelectedProduct(null);
                        setComment("");
                        setRating(5);

            } 
            catch (error) {
            console.error("Yorum gönderilemedi:", error);
            alert(error.response?.data?.message || "Yorum gönderilemedi."
            );
            }
                }}
            >
            Yorumu Gönder
            </button>

            <button
                className="cancel-review-btn"
                onClick={() => {
                setShowReviewForm(false);
                setSelectedProduct(null);
                }}
            >
                Vazgeç
            </button>
        </div>
    </div>
)}
    </div>
))
)}
    </div>
 );
}

    if (role === "Seller") {

    return (
        <div className="orders-container"> 
        <h2>Gelen Siparişler</h2>
         {orders.length === 0 ? ( 
            <p>Ürünlerinize henüz sipariş verilmedi.</p>
        ) : ( 
        orders.map(order => ( 
        <div
        className="order-card"
        key={`${order.orderId}-${order.product.id}`}>        <h3> Sipariş #{order.orderId} </h3> 
        <p> Tarih:{" "} 
            {new Date(order.orderDate).toLocaleString()} 
        267576777777777777777</p> 
                
            <p> Durum: 
            <span className={`status ${
            order.status === "Tamamlandı"
            ? "completed"
            : order.status === "İptal Edildi"
            ? "cancelled"
            : "preparing"
            }`}>
            {order.status}
            </span>
            </p> 
            {order.status !== "Tamamlandı" && (
    <button
        onClick={async () => {
            try {
                await axios.put(
                    `http://localhost:5070/api/Order/${order.orderId}/approve`
                );

                alert("Sipariş onaylandı.");

                const response = await axios.get(
                    `http://localhost:5070/api/Order/seller/${userId}`
                );

                setOrders(response.data);

            } catch (error) {
                console.error("Sipariş onaylanamadı:", error);
                alert("Sipariş onaylanamadı.");
            }
            }}
            >
            Siparişi Onayla
            </button>
            )}
            <hr /> 
            
            <div className="order-item"> 
                <img src={ order.product.imageUrl 
                ? "http://localhost:5070" + order.product.imageUrl 
                : "https://placehold.co/100x100?text=Ürün" } 
                alt={order.product.name} /> 
                
                <div> 
                <h4> {order.product.name} </h4> 
                <p> Adet: {order.quantity} </p> 
                <p> Birim Fiyat:{" "} {order.unitPrice} TL </p> 
                <p> Toplam:{" "} {order.totalPrice} TL </p> 
                </div> 
                </div> 
                </div> 
                )) 
            )} 
          </div> 
        ); 
    } 
    return null; 
}
export default Orders;