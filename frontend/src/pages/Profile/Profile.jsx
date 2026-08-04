import { useEffect, useState, useRef } from "react";
import { FaCamera } from "react-icons/fa";
import axios from "axios";
import "./Profile.css";

function Profile() {
  const [user, setUser] = useState(null);
  const [showForm, setShowForm] = useState(false);

  const [firstName, setFirstName] = useState("");
  const [lastName, setLastName] = useState("");
  const [username, setUsername] = useState("");
  const [email, setEmail] = useState("");
  const [age, setAge] = useState("");
  const [gender, setGender] = useState("");
  const [password, setPassword] = useState("");
  const fileInputRef = useRef(null);

  useEffect(() => {
    const loadUser = async () => {
    const storedUser = localStorage.getItem("user");

    if (!storedUser) return;

      const data = JSON.parse(storedUser);

      try {
        const response = await axios.get(
          `http://localhost:5070/api/User/${data.id}`
        );



  setUser(response.data);  
  console.log(user);
  setFirstName(data.firstName);
  setLastName(data.lastName);
  setUsername(data.username);
  setEmail(data.email);
  setAge(data.age);
  setGender(data.gender);
  }
    catch (error) {
    console.error(error);
  }
  };

  loadUser();

},[])


  const updateProfile = async () => {
  try {

    await axios.put(
    `http://localhost:5070/api/User/${user.id}`,
      {
        firstName,
        lastName,
        username,
        email,
        age: Number(age),
        gender,
        role: user.role,
        password
      }
    );
    const updatedUser = {
      ...user,
      firstName,
      lastName,
      username,
      email,
      age,
      gender
    };

    localStorage.setItem(
      "user",
      JSON.stringify(updatedUser)
    );

    setUser(updatedUser);

    setShowForm(false);

    alert("Profil güncellendi.");

  } 
  
  catch (error) {

  console.error(error);

  if (error.response?.data?.errors) {
    alert(error.response.data.errors.join("\n"));
  } else {
    alert(
      "Profil güncellenemedi: " +
      (error.response?.data?.message || "Bir hata oluştu.")
    );
  }

}
};


  const uploadPhoto = async (e) => {
  const file = e.target.files[0];

  if (!file) return;

  const formData = new FormData();
  formData.append("Image", file);

  try {
    const response = await axios.post(
      `http://localhost:5070/api/User/${user.id}/upload-photo`,
      formData,
      {
        headers: {
          "Content-Type": "multipart/form-data",
        },
      }
    );

    setUser({
      ...user,
      imageUrl: response.data.imageUrl,
    });

  } catch (error) {
    console.error(error);
    alert("Fotoğraf yüklenemedi.");
  }
};

  if (!user) 
    {
    return (
      <div className="profile-container">
        <h2>Giriş yapmanız gerekiyor.</h2>
      </div>
    );
  }

  return (
    <div className="profile-container">
      <div className="profile-card">
      <div className="profile-photo">

      <img
        src={
        user.imageUrl
        ? "http://localhost:5070" + user.imageUrl
        : "https://cdn-icons-png.flaticon.com/512/149/149071.png"
        }
        alt="Profil"
      />

    <button
      className="camera-btn"
      onClick={() => fileInputRef.current.click()}
      type="button"
      >
      <FaCamera />
    </button>

      <input
      type="file"
      accept="image/*"
      ref={fileInputRef}
      style={{ display: "none" }}
      onChange={uploadPhoto}
      />

    </div>

        <h2>
          {user.firstName} {user.lastName}
        </h2>

        <p className="role">
          {user.role}
        </p>

        <div className="profile-info">

          <div className="info-row">
            <span>Kullanıcı Adı</span>
            <strong>{user.username}</strong>
          </div>

          <div className="info-row">
            <span>E-posta</span>
            <strong>{user.email}</strong>
          </div>

          <div className="info-row">
            <span>Yaş</span>
            <strong>{user.age}</strong>
          </div>

          <div className="info-row">
            <span>Cinsiyet</span>
            <strong>{user.gender}</strong>
          </div>

        </div>

       <button
      className="edit-btn"
      onClick={() => setShowForm(!showForm)}
      >
      {showForm ? "İptal" : "Profili Düzenle"}
      </button>
      {showForm && (
    <div className="edit-form">

    <input
      value={firstName}
      onChange={(e) => setFirstName(e.target.value)}
      placeholder="Ad"
    />

    <input
      value={lastName}
      onChange={(e) => setLastName(e.target.value)}
      placeholder="Soyad"
    />

    <input
      value={username}
      onChange={(e) => setUsername(e.target.value)}
      placeholder="Kullanıcı Adı"
    />

    <input
      value={email}
      onChange={(e) => setEmail(e.target.value)}
      placeholder="Email"
    />

    <input
      type="number"
      value={age}
      onChange={(e) => setAge(e.target.value)}
      placeholder="Yaş"
    />

    <input
      value={gender}
      onChange={(e) => setGender(e.target.value)}
      placeholder="Cinsiyet"
    />

    <input
      type="password"
      value={password}
      onChange={(e) => setPassword(e.target.value)}
      placeholder="Yeni Şifre"
    />

    <button onClick={updateProfile}>
      Kaydet
    </button>

    </div>
)}
      </div>
    </div>
  );
}
export default Profile;