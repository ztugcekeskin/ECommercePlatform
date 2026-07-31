import { Link, useNavigate } from "react-router-dom";
import logo from "../../assets/logo.png";
import "./Navbar.css";

function Navbar() {

  const role = localStorage.getItem("role");
  console.log("Role:", role);
  const navigate = useNavigate();
  const handleLogout = () => {
    localStorage.clear();
    navigate("/login");
  };

  return (
    <header className="navbar">

      <div className="logo">
        <Link to="/">
          <img src={logo} alt="Logo" />
        </Link>
      </div>

      <div className="search">
        <input
          type="text"
          placeholder="Ürün ara..."
        />
      </div>

      <div className="right-menu">

  {!role && (
    <>
      <Link to="/login" className="login-link">
        Giriş Yap
      </Link>

      <Link to="/register" className="register-link">
        Kayıt Ol
      </Link>
    </>
  )}

  {role === "Customer" && (
    <>
      <Link to="/cart" className="cart-link">
        🛒 Sepet
      </Link>

      <Link to="/orders" className="login-link">
        Siparişlerim
      </Link>

      <Link to="/profile" className="login-link">
        Profil
      </Link>

      <button 
      className="logout-btn"
      onClick={handleLogout}
      >
        Çıkış Yap
      </button>
    </>
  )}

  {role === "Seller" && (
    <>
      <Link to="/my-products" className="login-link">
        Ürünlerim
      </Link>

      <Link to="/seller-orders" className="login-link">
        Siparişler
      </Link>

      <Link to="/chat" className="login-link">
        Mesajlar
      </Link>

      <Link to="/reviews" className="login-link">
        Yorumlar
      </Link>

      <Link to="/profile" className="login-link">
        Profil
      </Link>

      <button 
      className="logout-btn"
      onClick={handleLogout}
      >
        Çıkış Yap
      </button>
    </>
  )}

</div>

    </header>
  );
}
export default Navbar;