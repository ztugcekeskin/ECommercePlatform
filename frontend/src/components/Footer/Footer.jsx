import "./Footer.css";

function Footer() {
  return (
    <footer className="footer">
      <div className="footer-container">

        <div className="footer-section">
          <h3>MorSepet</h3>
          <p>
            Güvenli ve hızlı alışveriş deneyimi sunan e-ticaret platformu.
          </p>
        </div>

        <div className="footer-section">
          <h4>Kurumsal</h4>

          <a href="#">Hakkımızda</a>
          <a href="#">İletişim</a>
        </div>

        <div className="footer-section">
          <h4>Yardım</h4>

          <a href="#">Sık Sorulan Sorular</a>
          <a href="#">Kargo Takibi</a>
          <a href="#">İade Politikası</a>
        </div>

        <div className="footer-section">
          <h4>Bizi Takip Edin</h4>

          <a href="#">Instagram</a>
          <a href="#">Twitter</a>
          <a href="#">Tiktok</a>
        </div>

      </div>

      <div className="footer-bottom">
        © 2026 MorSepet | Tüm Hakları Saklıdır.
      </div>
    </footer>
  );
}
export default Footer;