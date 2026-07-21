import { Link } from 'react-router-dom';
import axios from "axios";
import { useState } from "react";
import "../styles/Login.css";

function Login() {

  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");

  const handleLogin = async () => {

    const user = {
      username: username,
      password: password
    };

    try {

      const response = await axios.post(
        "http://localhost:5070/api/Auth/login",
        user
      );

      console.log(response.data);

      alert(response.data.message);

    } catch (error) {

      console.error("Giriş hatası:", error);

      alert(
        "Giriş başarısız: " +
        (error.response?.data?.message || "Bir hata oluştu.")
      );
    }
  };


  return (
    <div className="login">

      <div className="login-card">

        <h2>Hoş Geldiniz</h2>

        <p className="subtitle">
          Hesabınıza giriş yapın
        </p>


        <div className="input-group">

          <label>Kullanıcı Adı</label>

          <input
            type="text"
            placeholder="Kullanıcı Adınızı Giriniz"
            value={username}
            onChange={(e) => setUsername(e.target.value)}
          />

        </div>


        <div className="input-group">

          <label>Şifre</label>

          <input
            type="password"
            placeholder="Şifrenizi Giriniz"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
          />

        </div>


        <div className="login-options">

          <label
            style={{
              display: 'flex',
              alignItems: 'center',
              gap: '8px',
              cursor: 'pointer'
            }}
          >

            <input type="checkbox" />

            Beni Hatırla

          </label>


          <a href="#forgot">
            Şifremi Unuttum?
          </a>

        </div>


        <button
          className="login-btn"
          onClick={handleLogin}
        >
          Giriş Yap
        </button>


        <div className="register">

          <p>
            Hesabın yok mu?
          </p>


          <Link
            to="/register"
            className="register-btn"
            style={{
              fontWeight: 'bold',
              marginTop: '8px',
              display: 'inline-block',
              textDecoration: 'none',
              textAlign: 'center'
            }}
          >
            Kayıt Ol
          </Link>


        </div>


      </div>

    </div>
  );
}

export default Login;