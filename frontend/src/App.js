// External Libraries
import { Routes, Route, Navigate } from "react-router-dom";

import CreateUserPage from './pages/CreateUserPage';
import TopBarPage from './components/TopBarPage';
import ProfilePage from './pages/ProfilePage';
import SearchPage from './pages/SearchPage';
import EditProfilePage from './pages/EditProfilePage';
import LoginPage from "./pages/LoginPage";
import AdminPage from "./pages/AdminPage";

import TestimonialPage from "./pages/TestimonialPage";


import CreateTestimonialPage from "./pages/CreateTestimonialPage";

import './App.css';

export default function App () {
  return (
    <Routes>
      <Route path="/login" element={<LoginPage/>} />
      <Route path="/create-user" element={<CreateUserPage/>} /> {/* Bør ændres til create-user */}
      <Route path="/topbar" element={<TopBarPage/>} />
      <Route path="/profile" element={<ProfilePage/>} /> 
      <Route path="/search" element={<SearchPage/>} />
      <Route path="/edit-profile" element={<EditProfilePage/>} />
      <Route path="/testimonial/" element={<TestimonialPage/>} />
      <Route path="/create/testimonials/:studentId" element={<CreateTestimonialPage/>} />
      <Route path="/" element={<Navigate to="login" replace/>} />
      <Route path="/admin" element={
        localStorage.getItem("role") ==="Admin"
              ?<AdminPage /> 
              :<Navigate to="/"/>
      }
      />
      <Route path="*" element={<div>404</div>} />
    </Routes>
  );
}