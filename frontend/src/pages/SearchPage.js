import { useState } from "react";
import TopBarPage from "../components/TopBarPage.js";
import "./SearchPage.css";
import { useNavigate } from "react-router-dom";

export default function SearchPage() {
    const [search, setSearch] = useState("");
    const [result, setResult] = useState([]);
    const [selectedClass, setSelectedClass] = useState("");
    const [classStudents, setClassStudents] = useState([]);
    const [selectedStudents, setSelectedStudents] = useState([]);

    const navigate = useNavigate();

    // SØGNING
    async function runSearch() {
        if (!search) {
            setResult([]);
            return;
        }

        const response = await fetch(
            `/api/students/search?name=${search}`
        );

        if (!response.ok) {
            setResult([]);
            return;
        }

        setResult(await response.json());
    }

    function handleSearch(e) {
        setSearch(e.target.value);
    }

    // VÆLG KLASSE
    async function selectClass(grade) {
        setSelectedClass(grade);

        if (!grade) {
            setClassStudents([]);
            return;
        }

        const response = await fetch(
            `/api/students/by-class/${grade}`
        );

        const studentsData = await response.json();

        if (Array.isArray(studentsData)) {
            setClassStudents(studentsData);
        } else {
            setClassStudents([]);
            console.error("Forventede et array, men fik:", studentsData);
        }
    }

    // GEM VALG OG GÅ VIDERE
    async function handleSubmit() {
        if (!selectedClass) return;

        const studentNamesToSend =
            selectedStudents.length > 0
                ? classStudents
                    .filter(s => selectedStudents.includes(s.studentId))
                    .map(s => ({
                        id: s.studentId,
                        name: `${s.firstName} ${s.lastName}`,
                        classId: s.classId
                    }))
            : classStudents.map(s => ({
                id: s.studentId,
                name: `${s.firstName} ${s.lastName}`,
                classId: s.classId
            }));

        navigate("/testimonial/", {
            state: {
                students: selectedStudents,
                classId: selectedClass,
                studentNames: studentNamesToSend
            }
        });
    }

    function getClassName(classId) {
        switch (classId) {
            case "1": return "2. klasse";
            case "2": return "4. klasse";
            case "3": return "6. klasse";
            case "4": return "9. klasse";
            default: return "";
        }
    }

    function toggleStudent(studentId) {
        setSelectedStudents(prev =>
            prev.includes(studentId)
                ? prev.filter(id => id !== studentId)
                : [...prev, studentId]
        );
    }

    function toggleSelectAll() {
        if (selectedStudents.length === classStudents.length) {
            setSelectedStudents([]);
        } else {
            setSelectedStudents(classStudents.map(s => s.studentId));
        }
    }

    // RETURN
    return (
        <main className="sp">
            <div className="sp-background"/>
            <section className="sp-content">

                <div className="create-user-container">
                    <TopBarPage />
                    <div className="search-card">

                        {/* SØGEFELT + KLASSEKNAPPER */}
                        <div className="search-top">
                            <input
                                id="searchInput"
                                type="text"
                                placeholder="Søg efter navn"
                                value={search}
                                onChange={handleSearch}
                                autoComplete="off"
                                onKeyDown={(e) => {
                                    if (e.key === "Enter") {
                                    e.preventDefault();
                                    runSearch();
                                }
                            }}
                            />

                            <div className="class-grid">
                                <button onClick={() => selectClass("1")} className={selectedClass === "1" ? "active" : ""}>2. klasse</button>
                                <button onClick={() => selectClass("2")} className={selectedClass === "2" ? "active" : ""}>4. klasse</button>
                                <button onClick={() => selectClass("3")} className={selectedClass === "3" ? "active" : ""}>6. klasse</button>
                                <button onClick={() => selectClass("4")} className={selectedClass === "4" ? "active" : ""}>9. klasse</button>
                            </div>

                            <button className="search-button" onClick={runSearch}>🔍</button>
                        </div>

                        {/* SØGERESULTAT */}
                        {result.length > 0 && (
                            <div className="result-box">
                                <h3>Søgeresultat</h3>
                                {result.map(student => (
                                    <div key={student.studentId} className="result-item">
                                        <input
                                            type="checkbox"
                                            checked={selectedStudents.includes(student.studentId)}
                                            onChange={async () => {
                                                setSelectedStudents([student.studentId]);
                                                setSelectedClass(student.classId);

                                                const response = await fetch(
                                                    `/api/students/by-class/${student.classId}`
                                                );
                                                setClassStudents(await response.json());
                                            }}
                                        />
                                        <span>{student.firstName} {student.lastName}</span>
                                    </div>
                                ))}
                            </div>
                        )}

                        {/* ELEVER I VALGT KLASSE */}
                        {selectedClass && result.length === 0 && (
                            <div className="result-box">
                                <h3>Elever i {getClassName(selectedClass)}</h3>

                                <div className="select-all">
                                    <input
                                        type="checkbox"
                                        checked={selectedStudents.length === classStudents.length && classStudents.length > 0}
                                        onChange={toggleSelectAll}
                                    />
                                    <label>Vælg alle</label>
                                </div>

                                {classStudents.map(student => (
                                    <div key={student.studentId} className="result-item">
                                        <input
                                            type="checkbox"
                                            checked={selectedStudents.includes(student.studentId)}
                                            onChange={() => toggleStudent(student.studentId)}
                                        />
                                        <span>{student.firstName} {student.lastName}</span>
                                    </div>
                                ))}
                            </div>
                        )}

                        {/* KNAP */}
                        <div className="button-row">
                            <button
                                disabled={selectedStudents.length === 0 || !selectedClass}
                                className="confirm-button"
                                onClick={handleSubmit}
                            >
                                Vælg
                            </button>
                        </div>
                    </div>
                </div>
            </section>
        </main>
    );
}

