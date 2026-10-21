import { getUserById } from "../services/userService.js";
import TopBarPage from "../components/TopBarPage.js";
import React, { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import "./ProfilePage.css";

// Resize-funktion
function resizeImage(file, maxSize = 600) {
    return new Promise((resolve) => {
        const img = new Image();
        const reader = new FileReader();

        reader.onload = (e) => (img.src = e.target.result);

        img.onload = () => {
            const canvas = document.createElement("canvas");
            let width = img.width;
            let height = img.height;

            if (width > height) {
                if (width > maxSize) {
                    height *= maxSize / width;
                    width = maxSize;
                }
            } else {
                if (height > maxSize) {
                    width *= maxSize / height;
                    height = maxSize;
                }
            }

            canvas.width = width;
            canvas.height = height;

            const ctx = canvas.getContext("2d");
            ctx.drawImage(img, 0, 0, width, height);

            canvas.toBlob((blob) => {
                const resizedFile = new File([blob], file.name, { type: file.type });
                resolve(resizedFile);
            }, file.type);
        };

        reader.readAsDataURL(file);
    });
}

// ⭐ Tilføj token til alle fetch-kald
function authHeader() {
    const token = localStorage.getItem("token");
    return {
        "Authorization": `Bearer ${token}`
    };
}

export default function ProfilePage() {
    const navigate = useNavigate();

    const [showConfirm, setShowConfirm] = useState(false);
    const [user, setUser] = useState(null);
    const [error, setError] = useState("");
    const [file, setFile] = useState(null);
    const [students, setStudents] = useState([]);
    const [teachers, setTeachers] = useState([]);
    const [userClasses, setUserClasses] = useState([]);

    const filteredStudents = students.filter(s => userClasses.includes(s.classLevel));

    // Hent bruger
    useEffect(() => {
        const userId = localStorage.getItem("userId");

        if (!userId) {
            setError("Ingen bruger fundet. Opret dig først.");
            return;
        }

        getUserById(userId)
            .then((data) => {
                setUser(data);
            })
            .catch((err) => {
                console.error("FEJL ved hentning af user:", err);
                setError(err.message);
            });
    }, []);

    // Hent klasser for brugeren
    useEffect(() => {
        if (!user?.id) return;

        if (user.role === "Admin" || user.roles?.includes("Admin")) {
            setUserClasses([]);
            return;
        }

        async function loadClasses() {
            try {
                const res = await fetch(
                    `/api/user/${user.id}/classes`,
                    { headers: authHeader() }
                );

                if (!res.ok) {
                    console.error("Fejl ved hentning af klasser:", res.status);
                    setUserClasses([]);
                    return;
                }

                const data = await res.json();
                setUserClasses(data);

            } catch (err) {
                console.error("Netværksfejl:", err);
                setUserClasses([]);
            }
        }

        loadClasses();
    }, [user]);

    // Hent undervisere
    useEffect(() => {
        fetch("/api/user/teachers", {
            headers: authHeader()
        })
            .then(res => res.json())
            .then(data => setTeachers(data));
    }, []);

    // Hent elever
    useEffect(() => {
        async function fetchStudents() {
            try {
                const res = await fetch("/api/students", {
                    headers: authHeader()
                });

                if (!res.ok) {
                    console.error("Serverfejl:", res.status);
                    setStudents([]);
                    return;
                }

                const data = await res.json();
                setStudents(data.map(s => ({
                    studentId: s.studentId,
                    firstName: s.firstName,
                    lastName: s.lastName,
                    classLevel: s.classId,
                    hasUploaded: s.hasUploaded ?? false
                })));

            } catch (err) {
                console.error("Fejl ved hentning af elever:", err);
                setStudents([]);
            }
        }

        fetchStudents();
    }, []);

    if (error) return <div>{error}</div>;
    if (!user) return <div>Indlæser profil...</div>;

    function handleEdit() {
        navigate("/edit-profile");
    }

    async function handleDelete() {
        const userId = localStorage.getItem("userId");

        const response = await fetch("/api/user/" + userId, {
            method: "DELETE",
            headers: authHeader()
        });

        if (response.ok) {
            alert("Bruger slettet");
            localStorage.clear();
            navigate("/");
        } else {
            alert("Fejl ved sletning af bruger");
        }

        setShowConfirm(false);
    }

    // Upload billede
    const handleUpload = async () => {
        if (!file) return;

        const resizedFile = await resizeImage(file, 600);

        const formData = new FormData();
        formData.append("file", resizedFile);

        const userId = localStorage.getItem("userId");

        try {
            const response = await fetch(
                `/api/user/upload-image/${userId}`,
                {
                    method: "POST",
                    headers: authHeader(),
                    body: formData,
                }
            );

            if (!response.ok) {
                alert("Fejl ved upload");
                return;
            }

            window.location.reload();
        } catch (error) {
            console.error("Upload-fejl:", error);
            alert("Der opstod en fejl under upload");
        }
    };

return (
    <main className="ptp">
        <div className="ptp-background" />
        <TopBarPage />

        <div className="ptp-content">

            {/* 🔥 3-KOLONNE GRID */}
            <div className="profile-grid">

                {/* KOLONNE 1 — USER */}
                <div className="card-box user-box">
                    <div className="profile-info">
                        <h2>{user.name}</h2>
                        <p>{user.role}</p>
                        <p>{user.email}</p>
                        <p>{user.phone}</p>
                    </div>
                </div>

                {/* KOLONNE 2 — ELEVER */}
                <div className="card-box student-list">
                    <h4>Dine elever:</h4>
                    <ul>
                        {filteredStudents.map(student => (
                            <li key={student.studentId} className="student-item">
                                <input type="checkbox" className="student-check" />
                                <span>
                                    {student.firstName} {student.lastName}

{/* DEBUG */}
        <span style={{ color: "red", marginLeft: "8px" }}>
            DEBUG: {String(student.hasUploaded)}
        </span>




                                    {student.hasUploaded && (
                                        <span className="upload-icon">📄</span>
                                    )}
                                </span>
                            </li>
                        ))}
                    </ul>
                </div>

                {/* KOLONNE 3 — VIDNESBYRD */}
                <div className="card-box testimonial-columns">

                    {/* Oploadet */}
                    <div className="uploaded-column">
                        <h2>Oploadet vidnesbyrd</h2>

                        {userClasses.map(cls => (
                            <div key={cls} className="class-section">
                                <h3>{cls}. klasse</h3>

                                {filteredStudents
                                    .filter(s => s.classLevel === cls && s.hasUploaded)
                                    .map(s => (
                                        <div key={s.studentId}>
                                            {s.firstName} {s.lastName}
                                            <span className="upload-icon">📄</span>
                                        </div>
                                    ))}

                                {filteredStudents.filter(s => s.classLevel === cls && s.hasUploaded).length === 0 && (
                                    <p>Ingen uploads endnu</p>
                                )}
                            </div>
                        ))}
                    </div>

                    {/* Manglende */}
                    <div className="missing-column">
                        <h2>Manglende Vidnesbyrd</h2>

 


                        {userClasses.map(cls => (
                            <div key={cls} className="class-section">
                                <h3>{cls}. klasse</h3>

                                {filteredStudents
                                    .filter(s => s.classLevel === cls && !s.hasUploaded)
                                    .map(s => (
                                        <div key={s.studentId}>
                                            {s.firstName} {s.lastName}
                                            <span className="missing-icon">⚠️</span>
                                        </div>
                                    ))}

                                {filteredStudents.filter(s => s.classLevel === cls && !s.hasUploaded).length === 0 && (
                                    <p>Alle har uploadet</p>
                                )}
                            </div>
                        ))}
                    </div>

                </div>
            </div>

            {/* ⭐ KNAPPERNE — UNDER KOLONNE 3 ⭐ */}
            <div className="bottom-row under-column-3">

                <div className="upload-section">
                    <label className="button button--secondary file-label">
                        Vælg fil
                        <input
                            type="file"
                            accept="image/*"
                            onChange={(e) => setFile(e.target.files[0])}
                        />
                    </label>

                    <button className="button button--primary" onClick={handleUpload}>
                        Upload billede
                    </button>
                </div>

                <div className="action-buttons">
                    <button className="button button--secondary" onClick={handleEdit}>
                        Ændre i profil
                    </button>
                    <button className="button button--secondary" onClick={() => setShowConfirm(true)}>
                        Slet profil
                    </button>
                </div>
            </div>

            {showConfirm && (
                <div className="confirm-box">
                    <p>Bekræft at du ønsker at slette din profil</p>
                    <button className="yes" onClick={handleDelete}>Ja</button>
                    <button className="no" onClick={() => setShowConfirm(false)}>Nej</button>
                </div>
            )}

        </div>
    </main>
);
}