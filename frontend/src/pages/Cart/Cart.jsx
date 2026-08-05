import { useEffect, useState } from "react";
import "./Cart.css";
import axios from "axios";

function Cart() {

    const [cartItems, setCartItems] = useState([]);

    const getCart = () => {
    const customerId = localStorage.getItem("userId");

    axios
    .get(`http://localhost:5070/api/Cart/${customerId}`)
    .then((response) => {
      setCartItems(response.data);
    })
    .catch((error) => {
      console.error(error);
    });
};

    const increaseQuantity = async (cartItemId) => {
    try {
    await axios.put(
      `http://localhost:5070/api/Cart/increase/${cartItemId}`
    );

    getCart();
  } catch (error) {
    console.error(error);
  }
}


const decreaseQuantity = async (cartItemId) => {
  try {
    await axios.put(
      `http://localhost:5070/api/Cart/decrease/${cartItemId}`
    );

    getCart();
  } catch (error) {
    console.error(error);
  }
};

  useEffect(() => {
  getCart();},[]);

  const totalPrice = cartItems.reduce(
  (total, item) => total + item.product.price * item.quantity,
  0
);

  return (
  <div className="cart-container">
    <h2>Sepetim</h2>

    {cartItems.length === 0 ? (
      <p>Sepetiniz boş.</p>
    ) : (
      <>
        {cartItems.map((item) => (
          <div className="cart-card" key={item.id}>
            <img
              src={
                item.product.imageUrl
                  ? "http://localhost:5070" + item.product.imageUrl
                  : "https://placehold.co/150x150?text=Ürün"
              }
              alt={item.product.name}
            />

            <div className="cart-info">
              <h3>{item.product.name}</h3>

              <p>{item.product.description}</p>

              <p>Fiyat: {item.product.price} TL</p>

              <div className="quantity-box">
                <button onClick={() => decreaseQuantity(item.id)}>
                  -
                </button>

                <span>{item.quantity}</span>

                <button onClick={() => increaseQuantity(item.id)}>
                  +
                </button>
              </div>

              <h4>
                Toplam: {(item.product.price * item.quantity).toFixed(2)} TL
              </h4>
            </div>
          </div>
        ))}

        <div className="cart-total">
          <h2>Genel Toplam: {totalPrice.toFixed(2)} TL</h2>

          <button className="checkout-btn">
            Siparişi Tamamla
          </button>
        </div>
      </>
    )}
  </div>
);
}
export default Cart;