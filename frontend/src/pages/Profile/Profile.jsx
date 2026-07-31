import { useEffect, useState } from "react";
import "./Profile.css";

function Profile() {
  const [user, setUser] = useState(null);

  useEffect(() => {
    const storedUser = localStorage.getItem("user");

    if (storedUser) {
      setUser(JSON.parse(storedUser));
    }
  }, []);

  if (!user) {
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
              user.imageUrl ||
              "https://cdn-icons-png.flaticon.com/512/149/149071.png"
            }
            alt="Profil"
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

        <button className="edit-btn">
          Profili Düzenle
        </button>

      </div>

    </div>
  );
}
export default Profile;