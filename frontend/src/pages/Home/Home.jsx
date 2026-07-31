import HomeCarousel from "../../components/Carousel/Carousel";
import "./Home.css";

function Home() {
  return (
    <div className="home">

      <HomeCarousel />

      <section className="hero">
        <h1>Hoş Geldiniz</h1>
      </section>
      
    </div>
  );
}

export default Home;