import { useEffect, useState } from "react";
import { useSearchParams } from "react-router-dom";
import axios from "axios";
import "./Product.css";

function Product() {
  const [products, setProducts] = useState([]);
  const [columns, setColumns] = useState(4);
  const [searchParams] = useSearchParams();
const searchTerm = searchParams.get("search") || "";

  useEffect(() => {
    axios
    .get(
    `http://localhost:5070/api/Product?search=${encodeURIComponent(searchTerm)}`
  )
      .then((response) => {
        setProducts(response.data);
      })
      .catch((error) => {
        console.error(error);
      });
  },[searchTerm]);
  
  
    const addToCart = async (productId) => {
  try {
    const customerId = localStorage.getItem("userId");

    await axios.post("http://localhost:5070/api/Cart", {
      customerId: Number(customerId),
      productId: productId,
      quantity: 1,
    });

    alert("Ürün sepete eklendi.");
  } catch (error) {
    console.error(error);
    alert("Ürün sepete eklenemedi.");
  }
};

  return (
    <div className="products-page">

      <div className="top-bar">
        <h2>Tüm Ürünler</h2>

        <div className="view-buttons">
          <button
            className={columns === 2 ? "active" : ""}
            onClick={() => setColumns(2)}
          >
            ☐☐ 2'li
          </button>
          <button
            className={columns === 4 ? "active" : ""}
            onClick={() => setColumns(4)}
          >
            ☐☐☐☐ 4'lü
          </button>
        </div>
      </div>

      {products.length === 0 ? (
        <p>Henüz ürün bulunmuyor.</p>
      ) : (
        <div
          className="products-grid"
          style={{
            gridTemplateColumns: `repeat(${columns}, 1fr)`,
          }}
        >
          {products.map((product) => (
            <div className="product-card" key={product.id}>
              <img
              src={product.imageUrl
              ? "http://localhost:5070" + product.imageUrl: "https://placehold.co/300x220?text=Ürün"
              }
              alt={product.name}
              />

              <div className="product-info">
                <h3>{product.name}</h3>

                <p>{product.description}</p>

                <p className="price">{product.price} TL</p>

                <p className="stock">
                  Stok: {product.stock}
                </p>

                <button
                  className="cart-btn"
                  disabled={product.stock === 0}
                  onClick={() => addToCart(product.id)}     
               >
                  {product.stock === 0 ? "Stokta Yok"
                  : "🛒 Sepete Ekle"}
                </button>
              </div>
            </div>
          ))}
        </div>
      )}
    </div>
  );
}
export default Product;