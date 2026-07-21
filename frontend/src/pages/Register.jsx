import { Link } from 'react-router-dom';
import axios from "axios";
import { useState } from "react";
import "../styles/Register.css";

function Register() {
  const [name, setName] = useState("");
  const [surname, setSurname] = useState("");
  const [age, setAge] = useState("");
  const [gender, setGender] = useState("");
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const [email, setEmail] = useState("");
  const register = async () => {
    const user = {
      name: name,
      surname: surname,
      age: Number(age),
      gender: gender,
      email: email,
      username: username,
      password: password
    };
    try {
      const response = await axios.post(
        "http://localhost:5070/api/Auth/register",
        user
      );
      console.log(response.data);
      alert(`Kayıt başarılı: ${response.data.message}`);
    } catch (error) {
      console.error("Kayıt hatası:", error);
      alert(
        "Kayıt başarısız: " +
        (error.response?.data?.message || "Kayıt sırasında bir hata oluştu.")
      );
    }
  };
  return (
    <div className="register-container">
      <div className="register-card">
        <h2>Kayıt Ol</h2>
        <p className="subtitle">
          Aşağıdaki bilgileri doldurup hesabınızı oluşturun
        </p>
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
        <div className="input-group">
          <label>Yaş</label>
          <input
            type="number"
            placeholder="Yaşınızı Giriniz"
            min="0"
            max="100"
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
            <option value="" disabled>
              Cinsiyet Seçiniz
            </option>
            <option value="erkek">
              Erkek
            </option>
            <option value="kadin">
              Kadın
            </option>
          </select>
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
          style={{ marginTop: '16px' }}
          onClick={register}
        >
          Kayıt Ol
        </button>
        <div
          className="login-redirect"
          style={{
            marginTop: '16px',
            textAlign: 'center'
          }}
        >
          <p>
            Zaten bir hesabın var mı?
          </p>
          <Link
            to="/"
            style={{
              fontSize: '16px',
              textDecoration: 'none',
              color: '#a855f7',
              fontWeight: 'bold',
              display: 'inline-block',
              marginTop: '8px',
            }}
          >
            Giriş Yap
          </Link>
        </div>
      </div>
    </div>
  );
} export default Register;