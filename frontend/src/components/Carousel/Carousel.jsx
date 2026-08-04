import BootstrapCarousel from "react-bootstrap/Carousel";
import "./Carousel.css";
import makeupSale from "../../assets/images/makyaj-indirim.jpg";
import backToSchool from "../../assets/images/okula-donus.jpg";
import hairAccessories from "../../assets/images/sac-aksesuar-indirim.jpg";

function HomeCarousel() {
  return (
    <BootstrapCarousel fade interval={3000}>
      <BootstrapCarousel.Item>
        <img
          className="carousel-image"
          src={makeupSale}
          alt="Makyaj İndirimi"
        />
      </BootstrapCarousel.Item>

      <BootstrapCarousel.Item>
        <img
          className="carousel-image"
          src={backToSchool}
          alt="Okula Dönüş"
        />
      </BootstrapCarousel.Item>

      <BootstrapCarousel.Item>
        <img
          className="carousel-image"
          src={hairAccessories}
          alt="Saç Aksesuar İndirimi"
        />
      </BootstrapCarousel.Item>
    </BootstrapCarousel>
  );
}
export default HomeCarousel;