import { useEffect, useState } from "react";
import TopBarPage from "../components/TopBarPage.js";
import { useNavigate } from "react-router-dom";
import "./EditProfilePage.css";

export default function EditProfilePage() {

    const [firstName, setFirstName] = useState("");
    const [lastName, setLastName] = useState("");
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");

    const navigate = useNavigate();

    // HENT BRUGERDATA
    useEffect(() => { 
        const userId = localStorage.getItem("userId");

        fetch(`/api/user/${userId}`)
            .then(res => res.json())
            .then(data => {
                setFirstName(data.firstName || "");
                setLastName(data.lastName || "");
                setEmail(data.email || "");
            });
    }, []);

    // OPDATER NAVN
    async function updateName() {
        const userId = localStorage.getItem("userId");

        await fetch(`/api/user/${userId}`, {
            method: "PUT",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({
                firstName: firstName,
                lastName: lastName
            })
        });

        alert("Navn opdateret");
    }

    // OPDATER EMAIL
    async function updateEmail() {
        const userId = localStorage.getItem("userId");

        await fetch(`/api/user/${userId}`, {
            method: "PUT",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({
                email: email
            })
        });

        alert("Email opdateret");
    }

    // OPDATER PASSWORD
    async function updatePassword() {
        const userId = localStorage.getItem("userId");

        await fetch(`/api/user/${userId}`, {
            method: "PUT",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({
                password: password,
                confirmPassword: password
            })
        });

        alert("Password opdateret");
    }

    // TILBAGE KNAP
    function navigateToProfile() {
        const role = localStorage.getItem("role");

        if (role && role.toLowerCase() === "admin") {
            navigate("/admin");
        } else {
            navigate("/profile");
        }
    }

    return (
        <div className="epp">
            <div className="epp-background"></div>

            <main className="epp-content">
                <TopBarPage />

                <div className="edit-card">
                    <h1>Rediger profil</h1>

                    <div className="edit-row">
                        <label>Fornavn:</label>
                        <input
                            type="text"
                            value={firstName}
                            onChange={(e) => setFirstName(e.target.value)}
                        />
                    </div>

                    <div className="edit-row">
                        <label>Efternavn:</label>
                        <input
                            type="text"
                            value={lastName}
                            onChange={(e) => setLastName(e.target.value)}
                        />
                        <button type="button" className="update-button" onClick={updateName}>
                            Opdater navn
                        </button>
                    </div>

                    <div className="edit-row">
                        <label>Email:</label>
                        <input
                            type="email"
                            value={email}
                            onChange={(e) => setEmail(e.target.value)}
                        />
                        <button type="button" className="update-button" onClick={updateEmail}>
                            Opdater email
                        </button>
                    </div>

                    <div className="edit-row">
                        <label>Password:</label>
                        <input
                            type="password"
                            value={password}
                            onChange={(e) => setPassword(e.target.value)}
                        />
                        <button type="button" className="update-button" onClick={updatePassword}>
                            Opdater password
                        </button>
                    </div>

                    <button
                        type="button"
                        className="back-button"
                        onClick={navigateToProfile}
                    >
                        Tilbage til profil
                    </button>
                </div>
            </main>
        </div>
    );
}
