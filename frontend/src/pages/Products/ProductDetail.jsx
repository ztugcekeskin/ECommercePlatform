import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import "./ProductDetail.css";
import axios from "axios";

function ProductDetail() {
  const { id } = useParams();

  const [product, setProduct] = useState(null);
  const [reviews, setReviews] = useState([]);

  useEffect(() => {
    // PostgreSQLdeki bilgi
    axios
      .get(`http://localhost:5070/api/Product/${id}`)
      .then((response) => {
        setProduct(response.data);
      })
      .catch((error) => {
        console.error("Ürün alınamadı:", error);
      });

    // MongoDBdeki bilgi
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
            <span className="stars">★★★★★</span>

            <span className="review-count">
              {reviews.length} yorum
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
    </div>);
}
export default ProductDetail;