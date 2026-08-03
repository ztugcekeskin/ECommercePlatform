import HomeCarousel from "../../components/Carousel/Carousel";
import Product from "../Products/Product";
import "./Home.css";

function Home() {
  return (
    <div className="home">

      <HomeCarousel />

      <section className="hero">
      <Product />      
      </section>
      
    </div>
  );
}

export default Home;