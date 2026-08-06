import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import axios from "axios";
import "./Payment.css";

function Payment() {

    const navigate = useNavigate();

    const [cartItems, setCartItems] = useState([]);

    useEffect(() => {

    const customerId = localStorage.getItem("userId");

    axios
        .get(`http://localhost:5070/api/Cart/${customerId}`)
        .then((response) => {
            setCartItems(response.data);
        })
        .catch((error) => {
            console.error(error);
        });

}, [])

    const totalPrice = cartItems.reduce(
    (total, item) => total + item.product.price * item.quantity,
    0
    );
    const handlePayment = async () => {

    const customerId = localStorage.getItem("userId");

    try {

        await axios.post(
            "http://localhost:5070/api/Order/checkout",
            {
                customerId: Number(customerId)
            }
        );

        alert("Ödeme başarılı!");

        navigate("/orders");

    } catch (error) {

        console.error(error);

        alert("Ödeme sırasında hata oluştu.");

    }

};

  return (
    <div className="payment-container">

      <h2>Ödeme</h2>

      <div className="payment-content">

  <div className="payment-card">

    <h3>Kart Bilgileri</h3>

    <input
      type="text"
      placeholder="Kart Sahibinin Adı"
    />

    <input
      type="text"
      placeholder="Kart Numarası"
    />

    <div className="card-row">

      <input
        type="text"
        placeholder="AA/YY"
      />

      <input
        type="password"
        placeholder="CVV"
      />

    </div>

  </div>

  <div className="summary-card">

    <h3>Sipariş Özeti</h3>

    {cartItems.map(item => (

      <div
        className="summary-item"
        key={item.id}
      >

        <span>
          {item.product.name} x {item.quantity}
        </span>

        <span>
          {(item.product.price * item.quantity).toFixed(2)} TL
        </span>

      </div>

    ))}

    <hr />

    <h2>
      Toplam: {totalPrice.toFixed(2)} TL
    </h2>

    <button
        className="checkout-btn"
        onClick={handlePayment}
>       Ödemeyi Tamamla
    </button>

  </div>

</div>

</div>

);
}
export default Payment;