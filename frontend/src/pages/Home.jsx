import { Link } from "react-router-dom";
import logo from "../assets/logo.png";
import "../styles/Home.css";

function Home() {
  return (
    <div className="home">
      <header className="navbar">
        <div className="logo">
          <img src={logo} alt="Logo" />
        </div>

        <div className="auth-buttons">
          <Link to="/login" className="login-link">
            Giriş Yap
          </Link>

          <Link to="/register" className="register-link">
            Kayıt Ol
          </Link>
        </div>
      </header>

      <nav className="menu">
        <a href="/">Ana Sayfa</a>
        <a href="#features">Özellikler</a>
        <a href="#about">Hakkımızda</a>
        <a href="#contact">İletişim</a>
      </nav>

      <div className="hero">
        <h1>Hoş Geldiniz</h1>

      </div>
    </div>
  );
}

export default Home;