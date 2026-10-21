import { useState } from "react";
import "./CreateUserPage.css";
import { createUserService } from "../services/userService.js";
import TopBarPage from "../components/TopBarPage.js";
import { useNavigate } from "react-router-dom";

export default function CreateUserPage() {
  const [successMessage, setSuccessMessage] = useState("");
  const [errorMessage, setErrorMessage] = useState("");
  const navigate = useNavigate();

  const [form, setForm] = useState({
    name: "",
    email: "",
    password: "",
  });

  async function handleSubmit(event) {
    event.preventDefault();

    setSuccessMessage("");
    setErrorMessage("");

    try {
      const result = await createUserService(form);

      localStorage.setItem("userId", result.id);
      navigate("/profile");

      setSuccessMessage("Bruger oprettet");
      console.log("Result:", result);
    } catch (error) {
      console.error("Create user failed:", error);
      setErrorMessage(error.message || "Fejl ved oprettelse af bruger");
    }
  }

  return (
    <div className="epp">
      <div className="epp-background"></div>

      <main className="epp-content">
        <div
          className="create-user-container"
          style={{ backgroundImage: "url('/background2.jpg')" }}
        >
          <TopBarPage />

          <div className="content-wrapper">
            <form className="create-user-form" onSubmit={handleSubmit}>
              {successMessage && (
                <div className="success-message">{successMessage}</div>
              )}
              {errorMessage && (
                <div className="error-message">{errorMessage}</div>
              )}

              <h1>Opret bruger</h1>

              <input
                type="email"
                placeholder="Email"
                value={form.email}
                onChange={(e) =>
                  setForm({ ...form, email: e.target.value })
                }
                required
              />

              <input
                type="password"
                placeholder="Adgangskode"
                value={form.password}
                onChange={(e) =>
                  setForm({ ...form, password: e.target.value })
                }
                required
              />

              <input
                type="text"
                placeholder="Navn"
                value={form.name}
                onChange={(e) =>
                  setForm({ ...form, name: e.target.value })
                }
                required
              />

              <button type="submit">Opret bruger</button>
            </form>
          </div>
        </div>
      </main>
    </div>
  );
}
