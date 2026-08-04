import { Link } from "react-router-dom";
import axios from "axios";
import { useState } from "react";
import "./Register.css";

function Register() {
  const [name, setName] = useState("");
  const [surname, setSurname] = useState("");
  const [age, setAge] = useState("");
  const [gender, setGender] = useState("");
  const [role, setRole] = useState("");
  const [email, setEmail] = useState("");
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");

  const register = async () => {
    const user = {
      name,
      surname,
      age: Number(age),
      gender,
      role,
      email,
      username,
      password,
    };
    try {
      const response = await axios.post(
        "http://localhost:5070/api/Auth/register",
        user
      );
      console.log(response.data);
      alert(`Kayıt başarılı: ${response.data.message}`);
    } 
    catch (error) {
  console.error("Kayıt hatası:", error);

  if (error.response?.data?.errors) {
    alert(error.response.data.errors.join("\n"));
  } else {
    alert(
      "Kayıt başarısız: " +
        (error.response?.data?.message || "Kayıt sırasında bir hata oluştu.")
    );
  }
}
  };

  return (

    <div className="register-container">
      <div className="register-card">
        <h2>Kayıt Ol</h2>
        <div className="form-row">
          <div className="input-group">
            <label>İsim</label>
            <input
              type="text"
              placeholder="İsminizi Giriniz"
              value={name}
              onChange={(e) => setName(e.target.value)}
            />
          </div>
          <div className="input-group">
            <label>Soyisim</label>
            <input
              type="text"
              placeholder="Soyisminizi Giriniz"
              value={surname}
              onChange={(e) => setSurname(e.target.value)}
            />
          </div>
        </div>
        <div className="form-row three">
          <div className="input-group">
            <label>Yaş</label>
            <input
              type="number"
              placeholder="Yaş"
              value={age}
              onChange={(e) => setAge(e.target.value)}
            />
          </div>
          <div className="input-group">
            <label>Cinsiyet</label>
            <select
              value={gender}
              onChange={(e) => setGender(e.target.value)}
            >
              <option value="">Seçiniz</option>
              <option value="Erkek">Erkek</option>
              <option value="Kadın">Kadın</option>
            </select>
          </div>
          <div className="input-group">
            <label>Rol</label>
            <select
              value={role}
              onChange={(e) => setRole(e.target.value)}
            >
              <option value="">Seçiniz</option>
              <option value="Customer">Müşteri</option>
              <option value="Seller">Satıcı</option>
            </select>
          </div>
        </div>
        <div className="input-group">
          <label>Email</label>
          <input
            type="email"
            placeholder="Email adresinizi giriniz"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
          />
        </div>
        <div className="input-group">
          <label>Kullanıcı Adı</label>
          <input
            type="text"
            placeholder="Kullanıcı adınızı giriniz"
            value={username}
            onChange={(e) => setUsername(e.target.value)}
          />
        </div>
        <div className="input-group">
          <label>Şifre</label>
          <input
            type="password"
            placeholder="Şifrenizi giriniz"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
          />
        </div>
        <button
          className="register-submit-btn"
          onClick={register}
        >
          Kayıt Ol
        </button>
        <div
          className="login-redirect"
          style={{ marginTop: "20px", textAlign: "center" }}
        >
          <p style={{ color: "white" }}>
            Zaten bir hesabın var mı?
          </p>
          <Link
            to="/login"
            style={{
              color: "#a855f7",
              fontWeight: "bold",
              textDecoration: "none",
            }}
          >
            Giriş Yap
          </Link>
        </div>
      </div>
    </div>
  );
}

export default Register;