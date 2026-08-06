import { useEffect, useState } from "react";
import axios from "axios";
import "./Orders.css";

function Orders() {

    const [orders, setOrders] = useState([]);

    useEffect(() => {

        const customerId = localStorage.getItem("userId");

        axios
            .get(`http://localhost:5070/api/Order/${customerId}`)
            .then((response) => {
                setOrders(response.data);
            })
            .catch((error) => {
                console.error(error);
            });

    }, []);

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
            <span className="status">
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
            </div>
            </div>
     ))}

    <h3 className="total-price">
    Toplam:
    {" "}
    {order.totalPrice} TL
    </h3>
    </div>
))
)}
    </div>
    );
}
export default Orders;