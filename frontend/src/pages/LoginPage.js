// External Libraries
import { useState } from "react";
import { useNavigate } from "react-router-dom";

// Local Files
import "./LoginPage.css";

export default function LoginPage() {

    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [message, setMessage] = useState("");

    const navigate = useNavigate();

    async function handleLogin(e) {
        e.preventDefault();
        setMessage("");

        try {
            const res = await fetch("/auth/login", {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({ email, password })
            });

            if (!res.ok) {
                let errorMessage = "Login failed";

                try {
                    const errJson = await res.json();
                    errorMessage = errJson.message || errorMessage;
                } catch {
                
                    console.error("Fejl i login ved auth try/catch:");
                }

                setMessage(errorMessage);
                return;
            }

            const data = await res.json();

            localStorage.setItem("token", data.token);
            localStorage.setItem("userId", data.userId);
            localStorage.setItem("role", data.role);

            setMessage("Login successful");

            if (data.role === "Admin") {
                navigate("/admin");
            } else {
                navigate("/profile");
            }

        } catch (err) {
            console.error(err);
            setMessage("Something went wrong."); //message on login page
        }
    }


    return (
        <main className="login-page">
            <div className="login-background" />

            <section className="login-flair">
                <h3>Nem og brugbar pdf-management website</h3>
                <p>Log ind for at få adgang til informationer, og for at kunne håndtere informationer</p>
            </section>

            <section className="login-card">
                <h1>Velkommen tilbage</h1>
                <p>Log ind for at fortsætte</p>  {/* Needs to be centered */}

                <form className="login-form" onSubmit={handleLogin}>
                    
                    <div className="form-field">
                        <label htmlFor="email">Email</label>
                        <input
                            id="email"
                            type="email"
                            placeholder="example@mail.com"
                            value={email}
                            onChange={(e) => setEmail(e.target.value)}
                            required
                        />
                    </div>

                    <div className="form-field">
                        <label htmlFor="password">Password</label>
                        <input
                            id="password"
                            type="password"
                            placeholder="***********"
                            value={password}
                            onChange={(e) => setPassword(e.target.value)}
                            required
                        />
                    </div>

                    <button type="submit">Log ind</button>

                    {message && <p className="login-message">{message}</p>}
                </form>
            </section>
        </main>
    );
}
