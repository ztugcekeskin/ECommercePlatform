import { BrowserRouter, Routes, Route } from "react-router-dom";
import Navbar from "./components/Navbar/Navbar";
import Home from "./pages/Home/Home";
import Login from "./pages/Login/Login";
import Register from "./pages/Register/Register";
import Profile from "./pages/Profile/Profile";
import MyProducts from "./pages/MyProducts/MyProducts";
import Footer from "./components/Footer/Footer";
import Cart from "./pages/Cart/Cart";
import Payment from "./pages/Payment/Payment";
import Orders from "./pages/Orders/Orders";
import ProductDetail from "./pages/Products/ProductDetail";

function App() {
  return (
    <BrowserRouter>

      <Navbar />

      <Routes>
        <Route path="/" element={<Home />} />
        <Route path="/home" element={<Home />} />
        <Route path="/login" element={<Login />} />
        <Route path="/register" element={<Register />} />
        <Route path="/profile" element={<Profile />} />
        <Route path="/my-products" element={<MyProducts />} />
        <Route path="/product/:id" element={<ProductDetail />}/>
        <Route path="/cart" element={<Cart />} />
        <Route path="/payment" element={<Payment />} />
        <Route path="/orders" element={<Orders />} />
        <Route path="/seller-orders" element={<Orders />} />
      </Routes>

      <Footer />

    </BrowserRouter>
  );
}
export default App;