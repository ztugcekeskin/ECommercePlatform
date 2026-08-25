import { NavLink, useNavigate } from "react-router-dom";
import { useEffect, useState } from "react";
import logo from "../../assets/logo.png";
import {FaHome,FaShoppingCart,FaBox,FaUser,FaSignInAlt,FaUserPlus} from "react-icons/fa";
import "./Navbar.css";

function Navbar() {

  const role = localStorage.getItem("role");
  console.log("Role:", role);
  const navigate = useNavigate();
  const handleLogout = () => {
    localStorage.clear();
    navigate("/login");
  };

  const [search, setSearch] = useState("");

useEffect(() => {
        if (search.trim() === "") {
            return;
        }
    const timer = setTimeout(() => {
    const encodedSearch = encodeURIComponent(search.trim());
      if (role === "Seller") {
          navigate(`/my-products?search=${encodedSearch}`);
      } else {
          navigate(`/?search=${encodedSearch}`);
      }
  }, 500);
  return () => {
      clearTimeout(timer);
  };
}, [search, role, navigate]);

  return (
    <header className="navbar">

      <div className="logo">
        <NavLink to="/">
          <img src={logo} alt="Logo" />
        </NavLink>
      </div>

      <div className="search">
        <input
          type="text"
          placeholder="Ürün ara..."
          value={search}
          onChange={(e) => setSearch(e.target.value)}
        />
      </div>

      <div className="right-menu">

  {!role && (
    <>
      <NavLink to="/login" className="login-link">
        Giriş Yap
      </NavLink>

      <NavLink to="/register" className="register-link">
        Kayıt Ol
      </NavLink>
    </>
  )}

  {role === "Customer" && (
    <>
    <NavLink
  to="/cart"
  className={({ isActive }) =>
    isActive ? "login-link active-link" : "login-link"
  }
  >
    <FaShoppingCart className="nav-icon" />
    Sepetim
    </NavLink>

    <NavLink
  to="/orders"
  className={({ isActive }) =>
    isActive ? "login-link active-link" : "login-link"
  }
  >
    <FaBox className="nav-icon" />
    Siparişlerim
    </NavLink>

    <NavLink
  to="/profile"
  className={({ isActive }) =>
    isActive ? "login-link active-link" : "login-link"
  }
  >
    <FaUser className="nav-icon" />
    Profilim
    </NavLink>

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
      <NavLink to="/my-products" className="login-link">
        Ürünlerim
      </NavLink>

      <NavLink to="/seller-orders" className="login-link">
        Siparişler
      </NavLink>

      <NavLink to="/chat" className="login-link">
        Mesajlar
      </NavLink>

      <NavLink to="/reviews" className="login-link">
        Yorumlar
      </NavLink>

      <NavLink to="/profile" className="login-link">
        Profil
      </NavLink>

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