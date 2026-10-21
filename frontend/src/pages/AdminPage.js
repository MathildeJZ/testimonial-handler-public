import "../pages/ProfilePage.css";
import "./AdminPage.css";

import React, { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import TopBarPage from "../components/TopBarPage";

export default function AdminPage() {
    const navigate = useNavigate();
    const role = localStorage.getItem("role");
    console.log("Rolle fra lokallager", role);

       //ClassId til klasserne
    const classes = [
        {id: 1, grade: 2},
        {id: 2, grade: 4},
        {id: 3, grade: 6},
        {id: 4, grade: 9},
    ];

    const [showConfirm, setShowConfirm] = useState(false);
    const [users, setUsers] = useState([]);
    const [user, setUser] = useState(null);
    const [file, setFile] = useState(null);
    const [teachers, setTeachers] = useState([]);
    const [successMessage, setSuccessMessage] = useState("");
    const [students, setStudents] =useState([]);
    const [studentTestimonials, setStudentTestimonials] = useState([]);
    const [showDeleteWarning, setShowDeleteWarning] = useState(false);
    const [selectedClassId, setSelectedClassId]  = useState(""); 



    const downloadTestimonials = (testimonials) => {
        console.log("Downloading testimonials:", testimonials);
        alert("Downloading funktion kommer senere");
    };


    const checkTestimonials = async (studentId) => {
        const token = localStorage.getItem("token");

        const res = await fetch(`/api/testimonials/student/${studentId}`, {
            headers: { "Authorization": "Bearer " + token }
        });

        if (res.status === 204) return [];

        const text = await res.text();
        if (!text) return [];

        return JSON.parse(text);
    };





const fetchStudentsForClass = (classId) => {
    const token = localStorage.getItem("token");

    fetch(`/api/students/by-class/${classId}`, {
        headers: { "Authorization": "Bearer " + token }
    })
        .then(async res => {
            console.log("STATUS:", res.status);
            const text = await res.text();
            console.log("RAW backend response:", text);

            try {
                return JSON.parse(text);
            } catch {
                console.error("Kunne ikke parse JSON:", text);
                return [];
            }
        })
        .then(data => {
            setStudents(data);
        })
        .catch(err => console.error("Fejl ved hentning af elever:", err));
};





const handleUpdateStudent = async () => {
    const token = localStorage.getItem("token");

    if (!selectedStudent.id) {
        alert("Vælg en elev først");
        return;
    }

    const updated = {
        firstName: selectedStudent.firstName,
        lastName: selectedStudent.lastName,
        school: selectedStudent.school,
        classId: Number(selectedStudent.classId)
    };

    console.log("Sender til backend:", updated);

    const response = await fetch(`/api/students/${selectedStudent.id}`, {
        method: "PUT",
        headers: {
            "Content-Type": "application/json",
            "Authorization": "Bearer " + token
        },
        body: JSON.stringify(updated)
    });

    if (response.ok) {
        alert("Elev opdateret!");

    await fetchStudentsForClass(selectedStudent.classId);
    } else {
        const error = await response.text();
        alert("Fejl ved opdatering: " + error);
    }
};

    const [selectedUser, setSelectedUser] = useState({
        id: null,
        firstName: "",
        lastName: "",
        email: "",
        profileImageUrl: "",
        isClassTeacher: false,
        isSubjectTeacher: false,
        classId: ""
    });


    const [selectedStudent, setSelectedStudent] = useState({
        id: null,
        firstName: "",
        lastName: "",
        classId: ""
    });

    const handleCreateStudent = async () => {
    const token = localStorage.getItem("token");

    const payload = {
        firstName: selectedStudent.firstName,
        lastName: selectedStudent.lastName,
        school: selectedStudent.school ?? "",
        classId: Number(selectedClassId)
    };

    console.log("Sender elev til backend:", payload);

    const response = await fetch("/api/students", {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
            "Authorization": "Bearer " + token
        },
        body: JSON.stringify(payload)
    });

    if (response.ok) {
        alert("Elev oprettet!");
        fetchStudentsForClass(selectedClassId);
        setSelectedStudent(null);
    } else {
        const error = await response.text();
        alert("Fejl ved oprettelse: " + error);
    }


    };

    const handleDeleteStudent = async () => {
        if (!selectedStudent || !selectedStudent.id) {
            alert("Vælg en elev først");
            return;
        }

        console.log("Tjekker testimonials for elev:", selectedStudent.id);

        const testimonials = await checkTestimonials(selectedStudent.id);

        console.log("Resultat:", testimonials);

        if (testimonials.length > 0) {
            setStudentTestimonials(testimonials);
            setShowDeleteWarning(true);
            return;
        }

        await deleteStudent(selectedStudent.id);
        alert("Eleven er slettet!");
        await fetchStudentsForClass(selectedClassId);
    };


        //slet elev
        const deleteStudent = async (id) => {
            const token = localStorage.getItem("token");

            try {
                const response = await fetch(`/api/students/${id}`, {
                    method: "DELETE",
                    headers: {
                        "Authorization": "Bearer " + token
                    }
                });

                if (!response.ok) {
                    const errorText = await response.text();
                    throw new Error("Kunne ikke slette eleven" + errorText);
                }

                // Fjern eleven fra UI
                setStudents(prev => prev.filter(s => s.id !== id));
                setSelectedStudent(null);
                setShowDeleteWarning(false);

            } catch (err) {
                console.error("Fejl ved sletning:", err);
            }
        };

    // Hent alle brugere
useEffect(() => {
    const token = localStorage.getItem("token");
    console.log("Token from localStorage:", token);

    fetch("/api/user", {
        headers: {
            "Authorization": "Bearer " + token
        }
    })
        .then(res => {
            console.log("GET /user status:", res.status);
            return res.json();
        })
        .then(data => {
            console.log("Loaded users:", data);
            setUsers(data);
        })
        .catch(err => console.error("Fejl ved hentning af brugere", err));
}, []);


    // Redirect hvis ikke admin
    useEffect(() => {
        if (!user) return;
            
        if(user.role !== "Admin") {
            navigate("/profile");
        }
    }, [user, navigate]);

    // Hent logget ind bruger
    useEffect(() => { 
        const userId = localStorage.getItem("userId");
        const token = localStorage.getItem("token");

        fetch(`/api/user/${userId}`, {
            headers: {
                "Authorization": "Bearer " + token
            }
        })
            .then(async res => {
                if (!res.ok) {
                    const text = await res.text();
                    console.error("Fejl:", text);
                    return;
                }
                return res.json();
            })
            .then(data => {
                if (!data) return;

                setSelectedUser({ 
                    id: data.Id || data.Id,
                    firstName: data.firstName || "",
                    lastName: data.lastName || "",
                    email: data.email || "",
                    profileImageUrl: data.profileImageUrl || "",
                    roles: [data.Role],
                    classId: data.classId || ""
                });
            });
    }, []);


      //hent alle lærere
    useEffect (() => {
        const token = localStorage.getItem("token");

        fetch("/api/user/teachers", {
            headers: {
                "Authorization": "Bearer " + token    
            }
        })
            .then(async res => {
                const text = await res.text();
                console.log("RAW backend response:", text);

                try {
                    return JSON.parse(text);
                } catch {
                    console.error("Kunne ikke parse JSON(teachers):", text);
                    return [];
                }
            })
            .then(data => {
                console.log("Loaded teachers:", data);
                setTeachers(data);
            
            })
            .catch(err => console.error("Fejl ved hentning af lærer", err));

    }, []);


    // Rolle-logik (bruges både til create og update)
    const buildRoles = () => {
        const roles = [];

        if (selectedUser.isClassTeacher) roles.push("ClassTeacher");
        if (selectedUser.isSubjectTeacher) roles.push("SubjectTeacher");

        if (roles.length === 0) roles.push("User");

        return roles;
    };

    // Opret bruger  opretter noget forkert!!! teacher i stedet for user. Også på updateteacher
    const handleCreateUser = async () => {
        const token = localStorage.getItem("token");

        const newUser = {
            firstName: selectedUser.firstName,
            lastName: selectedUser.lastName,
            email: selectedUser.email,
            password: "Password123!",
            confirmPassword: "Password123!",
            roles: buildRoles()
        };

        const response = await fetch("/api/user", {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                "Authorization": "Bearer " + token
            },
            body: JSON.stringify(newUser)
        });

        if (response.ok) {
            alert("Bruger oprettet");
            window.location.reload();
        } else {
            const error = await response.text();
            alert("Fejl ved oprettelse: " + error);
        }
    };

        const handleUploadUser = async () => {
        if (!file) {
            alert("Vælg et billede først");
            return;
        }

        const token = localStorage.getItem("token");
        const userId = localStorage.getItem("userId");

        const formData = new FormData();
        formData.append("file", file);

        const response = await fetch(`/api/user/upload-image/${userId}`, {
            method: "POST",
            headers: { "Authorization": "Bearer " + token },
            body: formData,
        });
        
        if(response.ok){
            // setSuccessMessage("Brugeren blev opdateret!");
            // setTimeout(() => setSuccessMessage(""), 3000);
        }else {
            const error = await response.json();
            alert("Fejl ved opdatering: " + JSON.stringify(error));
        }



        if (response.ok) {
            const data = await response.json();
            setUser(prev => ({ ...prev, profileImageUrl: data.imageUrl }));
            alert("Billede uploadet");
        } else {
            alert("Fejl ved upload");
        }
    };

    // Slet bruger
    const handleDeleteSelectedUser = async () => {
        const token = localStorage.getItem("token");

        if (!selectedUser.id) {
            alert("Vælg en bruger først");
            return;
        }

        const response = await fetch(`/api/user/${selectedUser.id}`, {
            method: "DELETE",
            headers: { "Authorization": "Bearer " + token }
        });

        if (response.ok) {
            alert("Bruger slettet");
            window.location.reload();
        } else {
            alert("Fejl ved sletning");
        }
    };
        // Opdater bruger  
const handleUpdateUser = async () => {
    
    const token = localStorage.getItem("token");

    const updatedUser = {
        firstName: selectedUser.firstName,
        lastName: selectedUser.lastName,
        email: selectedUser.email,
        roles: buildRoles()
    };

    // ⭐ Kun send password hvis brugeren faktisk har skrevet et nyt
    if (selectedUser.newPassword && selectedUser.newPassword.trim() !== "") {
        updatedUser.password = selectedUser.newPassword;
    }

    console.log("Payload:", updatedUser);

    const response = await fetch(`/api/user/${selectedUser.id}`, {
        method: "PUT",
        headers: {
            "Content-Type": "application/json",
            "Authorization": "Bearer " + token
        },
        body: JSON.stringify(updatedUser)
    });

    if (response.ok) {
        alert("Bruger opdateret");
        window.location.reload();
    } else {
        const error = await response.json();
        console.log("Backend fejl:", error);
        alert("Fejl ved opdatering: " + JSON.stringify(error));
    }
};



return (
    <div className="ap">
        <div className="ap-background"></div>

        <main className="ap-content">
            <div className="profile-container">
                <TopBarPage />

                {role === "Admin" && (
                    <div className="content-layout">
                        {/* Left Column – Profile Info */}
                        <div className="left-side">
                            {selectedUser?.profileImageUrl && (
                                <img
                                    src={`/${selectedUser.profileImageUrl}`}
                                    alt="Profilebillede"
                                    className="profile-image"
                                />
                            )}

                            <div className="profile-info">
                                <h2>{user?.firstName} {user?.lastName}</h2>
                                <p>{user?.email}</p>
                            </div>
                        </div>

                        {/* Middle Column – Users */}
                        <div className="card-box users-box">
                            <h4>Brugere</h4>

                            <ul className="teacher-list">
                                {users.map(u => (
                                    <li
                                        key={u.id}
                                        className="teacher-item"
                                        onClick={() =>
                                            setSelectedUser({
                                                id: u.id,
                                                firstName: u.firstName,
                                                lastName: u.lastName,
                                                email: u.email,
                                                profileImageUrl: u.profileImageUrl,
                                                isClassTeacher: u.isClassTeacher,
                                                isSubjectTeacher: u.isSubjectTeacher,
                                                classId: u.classId
                                            })
                                        }
                                    >
                                        {u.name}
                                    </li>
                                ))}
                            </ul>
                        </div>

                        {/* Middle Column – Students */}
                        <div className="card-box students-box">
                            <h4>Elever</h4>

                            {students.length === 0 && <p>Ingen elever fundet</p>}

                            <ul className="student-list">
                                {students.map(student => (
                                    <li
                                        key={student.studentId}
                                        className="student-item"
                                        onClick={() =>
                                            setSelectedStudent({
                                                id: student.studentId,
                                                studentId: student.studentId,
                                                firstName: student.firstName,
                                                lastName: student.lastName,
                                                school: student.school,
                                                classId: student.classId,
                                                hasUploaded: student.hasUploaded
                                            })
                                        }
                                    >
                                        {student.firstName} {student.lastName}
                                    </li>
                                ))}
                            </ul>
                        </div>

                        {/* Right Column – Create/Edit User */}
                        <div className="card-box create-user-box">
                            <h4>Opret bruger</h4>

                            <input
                                type="text"
                                placeholder="Fornavn"
                                value={selectedUser.firstName}
                                onChange={(e) =>
                                    setSelectedUser({ ...selectedUser, firstName: e.target.value })
                                }
                            />

                            <input
                                type="text"
                                placeholder="Efternavn"
                                value={selectedUser.lastName}
                                onChange={(e) =>
                                    setSelectedUser({ ...selectedUser, lastName: e.target.value })
                                }
                            />

                            <input
                                type="email"
                                placeholder="Email"
                                value={selectedUser.email}
                                onChange={(e) =>
                                    setSelectedUser({ ...selectedUser, email: e.target.value })
                                }
                            />

                            <div className="role-checkboxes">
                                <label>
                                    <input
                                        type="checkbox"
                                        checked={selectedUser.isClassTeacher}
                                        onChange={(e) =>
                                            setSelectedUser({
                                                ...selectedUser,
                                                isClassTeacher: e.target.checked
                                            })
                                        }
                                    />
                                    Klasselærer
                                </label>

                                <label>
                                    <input
                                        type="checkbox"
                                        checked={selectedUser.isSubjectTeacher}
                                        onChange={(e) =>
                                            setSelectedUser({
                                                ...selectedUser,
                                                isSubjectTeacher: e.target.checked
                                            })
                                        }
                                    />
                                    Faglærer
                                </label>
                            </div>

                            <div className="admin-buttons">
                                <button onClick={handleCreateUser}>Opret</button>
                                <button onClick={handleDeleteSelectedUser}>Slet</button>
                                <button onClick={handleUpdateUser}>Opdater</button>
                            </div>
                        </div>

                        {/* Right Column – Grade & Create/Edit Student */}
                        <div className="card-box class-box">
                            <h4>Årgang</h4>

                            <select
                                value={selectedClassId}
                                onChange={(e) => {
                                    const value = e.target.value;

                                    if (value === "") {
                                        setSelectedClassId("");
                                        setStudents([]);
                                        return;
                                    }

                                    const selected = classes.find(c => c.id === Number(value));
                                    const classId = selected.id;

                                    setSelectedClassId(classId);

                                    setSelectedStudent({
                                        id: null,
                                        firstName:"",
                                        lastName:"",
                                        school:"",
                                        classId: classId,
                                        hasUploaded: false
                                    });

                                    fetchStudentsForClass(classId);
                                }}
                            >
                                <option value="">Vælg årgang</option>
                                {classes.map(c => (
                                    <option key={c.id} value={c.id}>
                                        {c.grade}. årgang
                                    </option>
                                ))}
                            </select>

                            <input
                                type="text"
                                placeholder="Fornavn"
                                value={selectedStudent?.firstName ?? ""}
                                onChange={(e) =>
                                    setSelectedStudent(prev => ({ ...prev, firstName: e.target.value }))
                                }
                            />

                            <input
                                type="text"
                                placeholder="Efternavn"
                                value={selectedStudent?.lastName ?? ""}
                                onChange={(e) =>
                                    setSelectedStudent(prev => ({ ...prev, lastName: e.target.value }))
                                }
                            />

                            <div className="admin-buttons">
                                <button onClick={handleCreateStudent}>Opret</button>
                                <button
                                    onClick={() => {
                                        if (!selectedStudent) {
                                            alert("Vælg en elev først");
                                            return;
                                        }
                                        handleDeleteStudent(selectedStudent.id);
                                    }}
                                >
                                    Slet
                                </button>
                                <button onClick={handleUpdateStudent}>Opdater</button>
                            </div>

                            {showDeleteWarning && (
                                <div className="modal">
                                    <div className="modal-content">
                                        <h3>Eleven har et vidnesbyrd!</h3>
                                        <p>Vil du downloade vidnesbyrdet før du sletter eleven?</p>

                                        <button onClick={() => downloadTestimonials(studentTestimonials)}>
                                            Download vidnesbyrd
                                        </button>

                                        <button onClick={() => deleteStudent(selectedStudent.id)}>
                                            Slet alligevel
                                        </button>

                                        <button onClick={() => setShowDeleteWarning(false)}>
                                            Annuller
                                        </button>
                                    </div>
                                </div>
                            )}
                        </div>
                    </div>
                )}
            </div>
        </main>
    </div>
);
}
