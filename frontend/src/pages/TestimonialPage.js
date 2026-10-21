import { useEffect, useState } from "react";
import { useLocation, useNavigate } from "react-router-dom";
import TopBarPage from "../components/TopBarPage";
import "./TestimonialPage.css";

export default function TestimonialPage() {
    const API = "/api";
    const navigate = useNavigate();
    const location = useLocation();
    const navState = location.state || {};
    const role = getUserRole();

    // HENT FRA NAVIGATION
    const { 
        selectedStudents: initialStudents = [], 
        students = [], 
        studentNames = [] 
    } = navState;


    // STATE
    const [subjects, setSubjects] = useState([]);
    const [selectedSubjects, setSelectedSubjects] = useState([]);
    const [testimonials, setTestimonials] = useState([]);
    const [showPopup, setShowPopup] = useState(false);
    const [activeTestimonial, setActiveTestimonial] = useState(null);
    const [selectedStudents, setSelectedStudents] = useState(students);


    // STUDENT ID
    const studentId = students[0] ?? null;



    const handleOpenTestimonialPopup = async(id) => {
        const t = testimonials.find(x => x.testimonialId === id);
        setActiveTestimonial(t);
        setShowPopup(true);
    };



    // DERIVED VALUES
    const noData = !navState;
    // const students = navState?.students ?? [];
    const isSingle = students.length === 1;

    // HELPERS
    const classIdToClassName = (id) => {
        const map = {
            1: "2. klasse",
            2: "4. klasse",
            3: "6. klasse",
            4: "9. klasse",
        };
        return map[Number(id)] ?? "";
    };

    const classIdToGrade = (id) => {
        const map = {
            1: 2,
            2: 4,
            3: 6,
            4: 9,
        };
        return map[Number(id)] ?? null;
    };

    // INITIALIZE SELECTED STUDENTS
    useEffect(() => {
        if (studentNames.length > 0) {
            console.log("DEBUG -> Elevens classId:", studentNames[0].classId);
        }
    }, [studentNames]);

    // LOAD SUBJECTS BASED ON CLASS GRADE
    useEffect(() => {
        if (!navState) return;

        const grade = classIdToGrade(navState.classId);
        if (!grade) return;

        fetch(`/api/subjects/${grade}`)
            .then((res) => res.json())
            .then((data) => setSubjects(Array.isArray(data) ? data : []));
    }, [navState]);

    // TOGGLE STUDENT SELECTION
    const toggleStudent = (studentId) => {
        setSelectedStudents((prev) =>
            prev.includes(studentId)
                ? prev.filter((id) => id !== studentId)
                : [...prev, studentId]
        );
    };

    // MULTI-SELECT SUBJECTS
    const toggleSubject = (subjectId) => {
        setSelectedSubjects((prev) =>
            prev.includes(subjectId)
                ? prev.filter((id) => id !== subjectId) // fjern
                : [...prev, subjectId] // tilføj
        );
    };

    const loadStudentTestimonials = async (id) => {
        if(!id){
            console.error("Ingen studenterID modtaget");
            return;
        }
            const response = await fetch(`${API}/testimonials/student/${id}`);

            if(!response.ok) {
                console.error("Fejl i at hente vidnesbyrd:", response.status);
                return;
            }

        const data = await response.json();
        setTestimonials(data);
    };

    // LOAD TESTIMONIAL FOR SINGLE STUDENT
    const loadSingleTestimonial = async () => {
        const gradeName = classIdToClassName(studentNames[0].classId);

        const res = await fetch(
            `/api/testimonials?studentId=${selectedStudents[0]}&grade=${gradeName}&subjectIds=${selectedSubjects.join(",")}`
        );

        setTestimonials(await res.json());
    };

    // LOAD TESTIMONIALS FOR ENTIRE CLASS
    const loadAllClassTestimonials = async () => {
        console.log("🔥 FUNCTION CALLED")
        console.log("📤 selectedStudents:", selectedStudents);
        console.log("📤 selectedSubjects:", selectedSubjects);

        const classId = studentNames[0].classId;

        const res = await fetch(
            `/api/testimonials/class?classId=${classId}&subjectIds=${selectedSubjects.join(",")}`
        );

        setTestimonials(await res.json());
    };


useEffect(() => {
    if (!studentId) return;

    console.log("Henter vidnesbyrd for elev:", studentId);
    loadStudentTestimonials(studentId);

}, [studentId]);

console.log("DEBUG studentId:", studentId);


    // useEffect(() => {
    //     loadSelectedTestimonials();
    // }, [selectedStudents]);

    // NO DATA RECEIVED
    if (noData) {
        return (
            <div style={{ padding: "40px" }}>
                <h2>Ingen data modtaget</h2>
                <p>Gå tilbage til søgesiden og vælg en elev eller klasse.</p>
                <a href="/search">Tilbage til søgning</a>
            </div>
        );
    }

    // SELECT ALL STUDENTS
    const toggleSelectAll = () => {
        if (selectedStudents.length === studentNames.length) {
            setSelectedStudents([]);
        } else {
            setSelectedStudents(studentNames.map((s) => s.id));
        }
    };

    // FIXED SUBJECT BUTTONS
    const fixedSubjects = [
        { id: 1, name: "2. klasse" },
        { id: 2, name: "4. klasse" },
        { id: 3, name: "6. klasse" },
        { id: 4, name: "9. klasse" },
    ];

    const studentClassId = studentNames[0].classId;

    const allowedSubjects = fixedSubjects.filter(sub => sub.id <= studentClassId);


    function getUserRole(){
        const token = localStorage.getItem("token");
        if(!token) return null;

        const payload = JSON.parse(atob(token.split(".")[1]));

        return payload["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];
    }

return (
    <main className="tp">
        <div className="tp-background" />

        <section className="tp-content">
            <TopBarPage />

            <div className="sp-card">

                {/* HEADER */}
                <div className="top-header">
                    <h2 className="page-title">
                        {isSingle
                            ? studentNames[0].name
                            : classIdToClassName(studentNames[0].classId)}
                    </h2>

                    {!isSingle && (
                        <div>

                            {/* Checkbox */}
                            <div
                                style={{
                                    display: "flex",
                                    justifyContent: "flex-end",
                                    marginBottom: "10px",
                                    marginRight: "5px",
                                }}
                            >
                                <label style={{ display: "flex", alignItems: "center", gap: "6px" }}>
                                    <input
                                        type="checkbox"
                                        checked={selectedStudents.length === studentNames.length}
                                        onChange={toggleSelectAll}
                                    />
                                    Vælg/fravælg alle elever
                                </label>
                            </div>

                            {/* Elev-kasser */}
                            <div className="student-grid">
                                {studentNames.map((s) => (
                                    <div key={s.id} className="student-pill-wrapper">

                                        <button
                                            className={`student-pill ${selectedStudents.includes(s.id) ? "active" : ""}`}
                                            onClick={() => toggleStudent(s.id)}
                                        >
                                            {s.name}
                                        </button>

                                        {/* IKON: vises kun hvis der findes testimonials */}
                                        {testimonials.length > 0 &&
                                            selectedStudents.includes(s.id) && (
                                                <span
                                                    className="testimonial-icon"
                                                    onClick={() => handleOpenTestimonialPopup(s.id)}
                                                >
                                                    📄
                                                </span>
                                            )}
                                    </div>
                                ))}
                            </div>

                        </div>
                    )}
                </div>

                {/* SUBJECT SELECTOR */}
                <div className="class-box">
                    <h3>Vælg klassetrin</h3>

                    <div className="pill-container">
                        {allowedSubjects.map((s) => (
                            <button
                                key={s.id}
                                className={`pill ${selectedSubjects.includes(s.id) ? "active" : ""}`}
                                onClick={() => toggleSubject(s.id)}
                            >
                                {s.name}
                            </button>
                        ))}
                    </div>
                </div>

                {/* PREVIEW */}
                <div className="preview-box-wrapper">
                    <h3>
                        Preview
                        {testimonials.length > 0 && (
                            <span
                                className="testimonial-icon"
                                onClick={() => setShowPopup(true)}
                                style={{ marginLeft: "10px", cursor: "pointer" }}
                            >
                                📄
                            </span>
                        )}
                    </h3>

                    {testimonials.length > 0 ? (
                        <div className="preview-container">
                            {testimonials.map((t) => (
                                <div key={t.testimonialId} className="preview-item">
                                    <span
                                        className="testimonial-icon"
                                        onClick={() => handleOpenTestimonialPopup(t.testimonialId)}
                                    >
                                        📄
                                    </span>

                                        <p><strong>År:</strong> {t.schoolYear}</p>
                                        
                                        <p><strong>Titel:</strong> {t.title}</p>
                                </div>
                            ))}
                        </div>
                    ) : (
                        <p>Ingen vidnesbyrd at vise endnu...</p>
                    )}
                </div>


                {/* ACTION BUTTONS */}
                <div className="button-box">
                    <button
                        className="create-button"
                        onClick={() => loadStudentTestimonials()}
                    >
                        Download vidnesbyrd
                    </button>
                    {( role === "Admin" || role === "ClassTeacher") && (
                        <button
                            className="create-button"
                            onClick={() => navigate(`/create/testimonials/${selectedStudents[0]}`)}
                        >
                            Opret vidnesbyrd
                        </button>
                    )}
                    {( role === "SubjectTeacher") && (
                        <button
                            className="create-button"
                            onClick={() => navigate(`/create/testimonials/${selectedStudents[0]}`)}
                        >
                            Opret Fagbeskrivelse
                        </button>
                    )}


                    <button className="create-button">
                        Se vidnesbyrd
                    </button>
                </div>

                {/* POPUP VINDUE */}
                {showPopup && activeTestimonial && (
                    <div className="popup-overlay" onClick={() => setShowPopup(false)}>
                        <div className="popup-content" onClick={(e) => e.stopPropagation()}>
                            <h3>{activeTestimonial.title}</h3>
                            <p><strong>År:</strong> {activeTestimonial.schoolYear}</p>
                            <p>{activeTestimonial.testimonial}</p>

                            <button onClick={() => setShowPopup(false)}>Luk</button>
                        </div>
                    </div>
                )}


            </div>
        </section>
    </main>
);
}
