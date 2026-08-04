import HomeCarousel from "../../components/Carousel/Carousel";
import Product from "../Products/Product";
import "./Home.css";

function Home() {
  const role = localStorage.getItem("role");

  return (
    <div className="home">

      <HomeCarousel />

      {role === "Customer" && <Product />}
      {role === "Seller" && (
        <section className="hero">
          <h1>Satıcı Paneline Hoş Geldiniz</h1>
          <p>Ürünlerinizi yönetmek için yukarıdaki "Ürünlerim" sayfasını kullanabilirsiniz.</p>
        </section>
      )}
      {!role && (
        <section className="hero">
          <h1>Hoş Geldiniz</h1>
          <p>Ürünleri görmek için giriş yapabilirsiniz.</p>
        </section>
      )}
    </div>
  );
}

export default Home;