import { useEffect, useState } from "react";
import axios from "axios";
import "./MyProducts.css";

function MyProducts() {
  const [products, setProducts] = useState([]);
  const [showForm, setShowForm] = useState(false);

  const [name, setName] = useState("");
  const [description, setDescription] = useState("");
  const [price, setPrice] = useState("");
  const [stock, setStock] = useState("");
  const [imageUrl, setImageUrl] = useState("");
  const [selectedImage, setSelectedImage] = useState(null);

  const [columns, setColumns] = useState(4);
  const [editingId, setEditingId] = useState(null);

  const getProducts = () => {
    const sellerId = localStorage.getItem("userId");

    axios
      .get(`http://localhost:5070/api/Product/seller/${sellerId}`)
      .then((response) => {
        setProducts(response.data);
      })
      .catch((error) => {
        console.error(error);
      });
  };

  useEffect(() => {
    getProducts();
  }, []);

  const addProduct = async () => {
    try {
      const response = await axios.post(
  "http://localhost:5070/api/Product",
  {
    sellerId: Number(localStorage.getItem("userId")),
    name,
    description,
    price: Number(price),
    stock: Number(stock),
    imageUrl: null,
  }
);

const productId = response.data.productId;

if (selectedImage) {
  const formData = new FormData();

  formData.append("Image", selectedImage);

  await axios.post(
    `http://localhost:5070/api/Product/${productId}/upload-photo`,
    formData,
    {
      headers: {
        "Content-Type": "multipart/form-data",
      },
    }
  );
}

      alert("Ürün başarıyla eklendi.");

      setName("");
      setDescription("");
      setPrice("");
      setStock("");
      setImageUrl("");
      setSelectedImage(null);

      setShowForm(false);
      setEditingId(null);

      getProducts();
    } catch (error) {
      console.error(error);
      alert("Ürün eklenemedi.");
    }
  };

  const deleteProduct = async (id) => {

  if (!window.confirm("Ürünü silmek istiyor musunuz?"))
    return;

  try {

    await axios.delete(
      `http://localhost:5070/api/Product/${id}`
    );

    alert("Ürün silindi.");

    getProducts();

  } catch (error) {

    console.error(error);

    alert("Silinemedi.");

  }

};

const updateProduct = async () => {
  try {
    await axios.put(
  `http://localhost:5070/api/Product/${editingId}`,
  {
    sellerId: Number(localStorage.getItem("userId")),
    name,
    description,
    price: Number(price),
    stock: Number(stock),
    imageUrl: null,
  }
);

    if (selectedImage) {
  const formData = new FormData();

  formData.append("Image", selectedImage);

  await axios.post(
    `http://localhost:5070/api/Product/${editingId}/upload-photo`,
    formData,
    {
      headers: {
        "Content-Type": "multipart/form-data",
      },
    }
  );
}

    alert("Ürün güncellendi.");

    setEditingId(null);

    setName("");
    setDescription("");
    setPrice("");
    setStock("");
    setImageUrl("");
    setSelectedImage(null);
    setShowForm(false);
    setEditingId(null);

    getProducts();
  } 
  catch (error) {
  console.error(error);

  console.log(error.response);

  alert(error.response?.data?.message || error.response?.data || "Güncellenemedi.");
}
};

  return (
    <div className="my-products">
      <div className="top-bar">
        <h2>Ürünlerim</h2>

        <div className="view-buttons">
          <button onClick={() => setColumns(2)}>☐☐</button>

          <button onClick={() => setColumns(4)}>☐☐☐☐</button>
        </div>
      </div>

      <button
  className="add-product-btn"
  onClick={() => {

    if (!showForm) {
      setEditingId(null);
      setName("");
      setDescription("");
      setPrice("");
      setStock("");
      setImageUrl("");
    }

    setShowForm(!showForm);
  }}
>
  {showForm ? "İptal" : "+ Yeni Ürün Ekle"}
</button>

      {showForm && (
        <div className="product-form">
          <input
            type="text"
            placeholder="Ürün Adı"
            value={name}
            onChange={(e) => setName(e.target.value)}
          />
          <textarea
            placeholder="Açıklama"
            value={description}
            onChange={(e) => setDescription(e.target.value)}
          />
          <input
            type="number"
            placeholder="Fiyat"
            value={price}
            onChange={(e) => setPrice(e.target.value)}
          />
          <input
            type="number"
            placeholder="Stok"
            value={stock}
            onChange={(e) => setStock(e.target.value)}
          />
          <input
            type="file"
            accept="image/*"
            onChange={(e) => setSelectedImage(e.target.files[0])}
          />
            <button
            className="save-btn"
            onClick={editingId ? updateProduct : addProduct}
            >
            {editingId ? "Güncelle" : "Kaydet"}
          </button>
        </div>
      )}

      {products.length === 0 ? (
        <p>Henüz ürününüz bulunmuyor.</p>
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
                ? "http://localhost:5070" + product.imageUrl
                : "https://placehold.co/300x220?text=Ürün"
                }
                alt={product.name}
              />

              <div className="product-info">
                <h3>{product.name}</h3>

                <p>{product.description}</p>

                <p className="price">{product.price} TL</p>

                <p className="stock">Stok: {product.stock}</p>

                <div className="buttons">
               <button
                className="edit-btn"
                onClick={() => {
               
                setEditingId(product.id);

                setShowForm(true);

                setName(product.name);
                setDescription(product.description);
                setPrice(product.price);
                setStock(product.stock);
                setImageUrl(product.imageUrl);

                  window.scrollTo({
                  top: 0,
                  behavior: "smooth",
              });
            }}
           >
                    Düzenle
                  </button>

                  <button className="delete-btn" 
                  onClick={() => deleteProduct(product.id)}>
                    Sil
                  </button>
                </div>
              </div>
            </div>
          ))}
        </div>
      )}
    </div>
  );

}
export default MyProducts;