import { useState } from "react";
import { useNavigate } from "react-router-dom";
import "./TopBarPage.css";

export default function TopBarPage() {
    const [open, setOpen] = useState(false);
    const navigate = useNavigate();

    const role = localStorage.getItem("role");

    function handleLogout(e) {
        e.preventDefault();
        localStorage.clear();
        navigate("/login");
    }

    return (
        <div className="topbarpage-container">
            <img 
                src="/menuIconPe.png"
                alt="menu"
                className="burger-icon"
                onClick={() => setOpen(!open)}
            />

            {open && (
                <div className="dropdown-menu">
                    <a href="/login">Log ind</a>

                    {/* Dynamisk profil-link */}
                    <a href={role === "Admin" ? "/admin" : "/profile"}>
                        Min profil
                    </a>

                    <a href="/search">Søg</a>
                    <a href="/search">Fag</a>

                    <a href="/login" onClick={handleLogout}>Log ud</a>
                </div>
            )}
        </div>
    );
}

