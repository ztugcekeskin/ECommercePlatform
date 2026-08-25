import { useEffect, useState } from "react";
import axios from "axios";
import "./Reviews.css";

function Reviews() {

    const [reviews, setReviews] = useState([]);
    const userId = localStorage.getItem("userId");

    useEffect(() => {

        axios
        .get(`http://localhost:5070/api/Review/seller/${userId}`)
        .then((response) => {
            console.log("Seller reviews:", response.data);                setReviews(response.data);
        })
        .catch((error) => {
            console.error("Yorumlar alınamadı:", error);
        });
    }, [userId]);

    return (
    <div className="reviews-container">
        <h2>Ürünlerime Gelen Yorumlar</h2>

        {reviews.length === 0 ? (

            <p>Henüz ürünlerinize yorum yapılmamış.</p>
            ) : (reviews.map((review) => (
            <div className="seller-review-card">
            <div className="review-content">

        <h3>{review.userName}</h3>

        <div className="review-stars">
            {"★".repeat(review.rating)}
            {"☆".repeat(5 - review.rating)}
        </div>

        <p className="review-comment">
            {review.comment}
        </p>

        <small className="review-date">
            {new Date(review.createdAt).toLocaleDateString("tr-TR")}
        </small>

    </div>

    {review.product && (
        <div className="review-product">
            <img src={
                review.product.imageUrl
                ? "http://localhost:5070" +
                  review.product.imageUrl
                : "https://placehold.co/120x120?text=Ürün"
            }
            alt={review.product.name}
        />

        <div className="review-product-info">

            <h4>{review.product.name}</h4>

            <p>{review.product.price} TL</p>

            </div>
        </div>
        )}
    </div>
    ))
)}
</div>
);}
export default Reviews;