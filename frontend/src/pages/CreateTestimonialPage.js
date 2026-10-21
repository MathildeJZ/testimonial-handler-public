// Local libraries
import { createSubjectEntry } from "../services/subjectEntryService";
import { createTestimonial } from "../services/testimonialServices";
import TopBarPage from "../components/TopBarPage.js";
import { getByStudentId } from "../services/studentServices"
import "./CreateTestimonialPage.css";
// External Libraries
import { useParams } from "react-router-dom";
import { useEffect, useState } from "react";



export default function CreateTestimonialPage(){

//_____________________________________________________________________________________________
// STATE FIRST IN FILE
    const role = getUserRole();

    const [selectedSubjectIndex, setSelectedSubjectIndex] = useState(0);
    
    const [student, setStudent] = useState(null);
    const [selectedBlocks, setSelectedBlocks] = useState([]);

    const [teachers, SetTeachers] = useState([]);


    const [teacherId, setTeacherId] = useState(null);
    const [mySubjects, setMySubjects] = useState([]);
    const [subjects, setSubjects] = useState([]);
    const [subjectId, setSubjectId] = useState(null)

    const [statusMessage, setStatusMessage] = useState(null);
    const [isError, setIsError] = useState(false);

    const [isLoading, setIsLoading] = useState(false);

    
    const [subjectEntries, setSubjectEntries] = useState([
        {
            subjectId: null,
            subjectName: "",
            teacherId: null,
            descriptionBlocks: [],
            grade: ""
        }
    ]);

    const [formData, setFormData] = useState({
        overallStudentDescription: "",
        endMessage: "",
        subjects: [
            {
                subjectId: 22,
                subjectName: "Dansk",
                teacherName: "",
                studentStatement: "",
                descriptionBlocks: [
                    {
                        heading: "",
                        body: "",
                    }
                ]
            },
            {
                subjectId: 23,
                subjectName: "Engelsk",
                teacherName: "",
                studentStatement: "",
                descriptionBlocks: [
                    {
                        heading: "",
                        body: "",
                    }
                ]
            },
            {
                subjectId: 31,
                subjectName: "Matematik",
                teacherName: "",
                studentStatement: "",
                descriptionBlocks: [
                    {
                        heading: "",
                        body: "",
                    }
                ]
            },
            {
                subjectId: 40,
                subjectName: "Idræt",
                teacherName: "",
                studentStatement: "",
                descriptionBlocks: [
                    {
                        heading: "",
                        body: "",
                    }
                ]
            }
        ]
    });


    //find selected student
    const { studentId } = useParams();

    useEffect(() => {
        async function loadStudent() {
            try {
                const data = await getByStudentId(studentId);

                console.log("API DATA:", data);

                setStudent(data);
            } catch(err) {
                console.error("🎀Faild to load student:🎀", err)
            } 
        }
        loadStudent();
    }, [studentId]);


//_____________________________________________________________________________________________
// USEEFFFECTS
    //hent teachers til dropdown
    useEffect(() => {

        fetch("/api/teachers")
        .then(res => res.json())
        .then(data => {
            SetTeachers(data);
        })
        .catch(err => console.error(err));
            
    }); 

// fecth subjects for subjectTeacher
    useEffect(() =>{
        if(role === "SubjectTeacher"){
            const fetchMySubjects = async () => {
            try{
                const token = localStorage.getItem("token");

                const res = await fetch("/api/testimonials/me", {
                    method: "GET",
                    headers: {
                        "Content-Type": "application/json",
                        "Authorization": `Bearer ${token}`
                    }
                });
                console.log("📡 STATUS:", res.status)

                if(!res.ok) {
                    const text = await res.text();
                    console.error("💀 ERROR RESPONSE:", text);
                    throw new Error("Failed to fetch teacher subjects");
                }
                
                const data = await res.json();

                console.log("📚MY SUBJECTS:📚", data)
                
                setMySubjects(data.subjects);
                setFormData((prev) => ({
                    ...prev,
                    subjects: data.subjects.map((s) => ({
                        subjectId: s.subjectId,
                        subjectName: s.name,
                        teacherId: null,
                        descriptionBlocks: []
                    }))
                }))

            } catch (err) {
                console.error("🔥 FAILED to fetch teacher data:", err)
            }


        };
        fetchMySubjects();
    }
    }, [role])

    useEffect(() => {
        if(formData.subjects?.length) {
            const initialEntries = formData.subjects.map(s => ({
                subjectId: s.subjectId,
                teacherId: null,
                descriptionBlocks: []
            }));

            setSubjectEntries(initialEntries);
        }
    }, []);

//_____________________________________________________________________________________________
// CONSTANTS    
    const addDescriptionBlock = (subjectIndex) => {
        setFormData(prev => {

            const subjects = [...prev.subjects];
            const subject = subjects[subjectIndex];

            subjects[subjectIndex] = {
                ...subject,
                descriptionBlocks: [
                    ...blocks,
                    { heading: "", body: "" }
                ]
            };

            return { ...prev, subjects };
        });        
    };


    const deleteSelectedBlocks = () => {
        if (selectedBlocks.length === 0) return;

        setFormData(prev => {
            const subjects = [...prev.subjects];
            const subject = subjects[selectedSubjectIndex];

            const remaining = subject.descriptionBlocks.filter(
            (_, i) => !selectedBlocks.includes(i)
            );

            if (remaining.length === 0) return prev;

            subjects[selectedSubjectIndex] = {
            ...subject,
            descriptionBlocks: remaining
            };

            return { ...prev, subjects };
        });

        setSelectedBlocks([]);
    };


    const selectedSubject = formData.subjects[selectedSubjectIndex]

    const blocks = selectedSubject?.descriptionBlocks ?? [];
    

    const handleSubjectEntryChange = (index, field, value) => {
        // console.log("🎯 CHANGE CALLED:", index, field, value);

        const updatedEntries = [...subjectEntries];

        if(!updatedEntries[index]) {
            updatedEntries[index] = {};
        }

        updatedEntries[index][field] = value;

        console.log("🧪 AFTER UPDATE:", updatedEntries);

        setSubjectEntries(updatedEntries);
    }

    const current = subjectEntries[selectedSubjectIndex];

    const classYearMap = {
        1: 2,
        2: 4,
        3: 6,
        4: 9
    };


//_____________________________________________________________________________________________
// FUNCTIONS
    function updateSubject(subjectIndex, updater) {
        setFormData(prev => { 
            const updatedSubjects = [...prev.subjects];
            updatedSubjects[subjectIndex] = updater(updatedSubjects[subjectIndex]);

            return {
                ...prev, 
                subjects: updatedSubjects 
            };
        });
    }

    function updateDescriptionBlock (subjectIndex, blockIndex, updater) {
        updateSubject(subjectIndex, subject => {
            const updatedBlocks = [...subject.descriptionBlocks];
            updatedBlocks[blockIndex] = updater(updatedBlocks[blockIndex]);

            return{
                ...subject,
                descriptionBlocks: updatedBlocks
            };
        });
    }

    const toggleBlock = (blockIndex) => {
        setSelectedBlocks(prev => 
            prev.includes(blockIndex)
            ? prev.filter(i => i !== blockIndex)
            : [...prev, blockIndex]
        );
    };

    function getUserRole(){
        const token = localStorage.getItem("token");
        if(!token) return null;

        const payload = JSON.parse(atob(token.split(".")[1]));

        return payload["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];
    }

    async function handleCreateTestimonial(payload) {

        if (!student) {
            console.log("Student not loaded yet");
            return;
        }

        try{
            const payload = {
                studentId: studentId,
                title: "Vidnesbyrd",
                grade: "9A",
                schoolYear: 2026,
                
                overallStudentDescription: formData.overallStudentDescription || "Test description",
                endMessage: formData.endMessage || "Test end message",
                
                subjects: subjectEntries.map((s, index) => ({
                    subjectId: s.subjectId,
                    teacherId: s.teacherId,
                    subjectDescription: (s.descriptionBlocks || [])
                    .map(block => `${block.heading} ${block.body}`)
                    .join("\n")
                }))
            };

            setIsLoading(true);
            
            const result = await createSubjectEntry(payload);
            console.log("❌", result)

            setStatusMessage("✅ PDF blev oprettet!");
            setIsError(false);

        } catch(error) {
            setStatusMessage("Noget gik galt ved oprettelse af PDF");
            setIsError(true);

        } finally{
            setIsLoading(false);
        }
    }

    async function handleSubmitSubjectEntry() {
        console.log("🦝SUBMIT SUBEJCTENTRY ATTEMPTED🦝")
        const entry = formData.subjects[selectedSubjectIndex];

        const payload = {
            studentId,
            subjectId: entry?.subjectId,
            personalStatement: entry?.studentStatement,
            descriptionBlocks: entry?.descriptionBlocks,
        };
        console.log("🥝ENTRY🥝", entry);

        try{
            setIsLoading(true);
            
            const result = await createSubjectEntry(payload);
            console.log("🍄🍄🍄", result)

            setStatusMessage("✅ Fagbeskrivelse blev oprettet!");
            setIsError(false);

        } catch(error) {
            setStatusMessage("Noget gik galt ved oprettelse af fagbeskrivelse");
            setIsError(true);
            console.log("🍎ENTRY🍎", entry);

        } finally{
            setIsLoading(false);
        }

        console.log("🫧🫧PAYLOAD:🫧🫧", payload)
    }

    async function handleSaveDraft() {
        const subject = formData.subjects(selectedSubjectIndex)
        const payload = {
            studentId, 
            subjectName: subject?.subjectName, 
            teacherName: subject?.teacherName,
            blocks: subject?.blocks,
            endMessage : formData.endMessage
        };

        await fetch("/api/Subjects/draft", {
            method: "POST", 
            headers: {
                "Content-Type": "application/json",
                "Authorization": `Bearer ${localStorage.getItem("token")}`
            },
            body: JSON.stringify(payload)
        })

    }

    //__________________________________________________________________________________________________________________________
    //SIDEN//
    return (
        <main className="ctp">
            <div className="ctp-background"/>
            <TopBarPage/>
            <section className="ctp-content">
                <header className="ctp-header">
                    <div>
                        {( role === "Admin" || role === "ClassTeacher") && (
                            <h1> Opret vidnesbyrd</h1>
                        )}
                        {( role === "SubjectTeacher") && (
                            <h1> Tilføj fagbeskrivelse til vidnesbyrd </h1>
                        )}
                        <p> Udfyld sektionerne, og opret vidnesbyrdet som en PDF-fil.</p>
                        
                    </div>
                    <div className="ctp-actions">
                        {( role === "Admin" || role === "ClassTeacher") && (
                            <button 
                                className="btn btn--primary" 
                                type="button" 
                                onClick={handleCreateTestimonial} 
                                disabled={!student || isLoading}
                            > 
                                {isLoading ? "Opretter..." : "Opret PDF"}
                            </button>
                        )}

                        {statusMessage && (
                            <div style={{marginTop: "10px", background: "#b3b6bb",color: "#3771cf", padding: "10px", borderRadius: "6px"}}>
                                {statusMessage}
                            </div>
                        )}
                    </div>
                </header>

                <section className="ctp-grid">
                    {/* Right Column */}
                    <aside className="ctp__sidebar">
                        <div className="card">
                            <h2 className="card__title">Elev</h2>
                            <div className="card__row"><span className="muted">Navn</span><span>{student?.firstName} {student?.lastName}</span></div>
                            <div className="card__row"><span className="muted">Klasse</span><span>{classYearMap[student?.classId]} klasse</span></div>
                            {/* <div className="card__row"><span className="muted">Årgang</span><span>{selectedStudent.}</span></div> */}
                            
                        </div>
                    </aside>

                    {/* Right Column */}
                    <div>
                    {( role === "Admin" || role === "ClassTeacher") && ( 
                        <div className="ctp__content">
                            <div className="card">
                                <h2 className="card__title">Overordnet beskrivelse</h2>
                                <textarea className="input" 
                                    rows={6}
                                    placeholder="Klasselærens overordnede beskrivelse af eleven..."
                                    value={formData.overallStudentDescription}
                                    onChange={(e) => 
                                        setFormData( prev => ({ 
                                            ...prev,
                                            overallStudentDescription: e.target.value
                                        }))
                                    }
                                />
                        </div>
                        <div className="card">
                            <h2 className="card__title"> Afslutende besked </h2>
                            <textarea className="input" 
                                rows={3} 
                                placeholder="Vi ønsker..." 
                                value={formData.endMessage}
                                onChange={(e) => 
                                    setFormData({
                                        ...formData,
                                        endMessage: e.target.value
                                    })
                                }
                            />
                        </div>
                    </div>
                    )}
                        
                    <div className="card">
                        {( role === "Admin" || role === "ClassTeacher") && (
                            <div>
                                <h2 className="card__title">Fag</h2>

                                <div className="subjectList">
                                    {formData.subjects.map((s, subjectIndex) => (
                                        <button
                                            key={s.subjectName + subjectIndex}
                                            type="button"
                                            className={
                                                "subjectList__item " + 
                                                (subjectIndex === selectedSubjectIndex ? "subjectList__item--active" : "")
                                            }
                                            onClick={() => setSelectedSubjectIndex(subjectIndex)}
                                        >
                                            {s.subjectName}
                                        </button>
                                    ))}
                                </div>
                                

                                <select
                                    className="select"
                                    value={current?.teacherId?.toString() ?? ""}
                                    onChange={(e) => {
                                        const parsed = e.target.value ? parseInt(e.target.value) : null;

                                        console.log("👉 Selected teacherId:", parsed);

                                        handleSubjectEntryChange(
                                            selectedSubjectIndex, 
                                            "teacherId", 
                                            parsed
                                        );
                                    }}
                                >
                                    <option value=""> Vælg lærer </option>
                                    {teachers.map((teacher) => (
                                        <option key={teacher.teacherId} value={teacher.teacherId}>
                                            {teacher.firstName} {teacher.lastName}
                                        </option>
                                    ))}
                                </select>
                            </div>
                        )}
                        { role === "SubjectTeacher" && (
                            <div>
                                <h2>Dine fag</h2>
                                <div className="subjectList">
                                    {mySubjects.map((s, index) => (
                                        <button
                                            key={s.subjectId}
                                            type="button"
                                            className={
                                                "subjectList__item " +
                                                (index === selectedSubjectIndex ? "subjectList__item--active" : "")
                                            }
                                            onClick={() => {
                                                setSelectedSubjectIndex(index);
                                                handleSubjectEntryChange(
                                                    index,
                                                    "subjectId",
                                                    s.subjectId
                                                );
                                            }}
                                        >
                                            {s.name}
                                        </button>
                                    ))}
                                </div>
                            </div>
                        )}

                        <button 
                            className="btn btn--secondary" 
                            type="button" 
                            onClick={handleSaveDraft}
                        > 
                            Gem kladde 
                        </button>
                        <button 
                            className="btn btn--secondary" 
                            type="button" 
                            onClick={handleSubmitSubjectEntry}
                            disabled={isLoading}
                        > 
                            Indsend fagbeskrivelse 
                        </button>
                    </div>

                        {blocks.map((block, blockIndex) => (
                            <div key={blockIndex} className="descriptionBlock">
                                <div className="blockHeader">
                                    <input
                                        type="checkbox"
                                        checked={selectedBlocks.includes(blockIndex)}
                                        onChange={() => toggleBlock(blockIndex)}
                                    />
                                </div>

                                <div className="split">
                                    <div>
                                        <label className="label">Fagbeskrivelse overskrift</label>

                                        <input
                                            className="input"
                                            placeholder="fx Hovedfagsperiode..."
                                            value={block.heading}
                                            onChange={(e) => {
                                                updateDescriptionBlock(selectedSubjectIndex, blockIndex, b => ({
                                                    ...b,
                                                    heading: e.target.value
                                                }));
                                            }}
                                        />
                                    </div>

                                    <div>
                                        <label className="label">Fagbeskrivelse tekst</label>

                                        <textarea
                                            className="input"
                                            rows={4}
                                            placeholder="Beskrivelse..."
                                            value={block.body}
                                            onChange={(e) => {
                                                updateDescriptionBlock(selectedSubjectIndex, blockIndex, b => ({
                                                    ...b,
                                                    body: e.target.value
                                                }));
                                            }}
                                        />
                                    </div>
                                </div>
                            </div>
                        ))}
                                
                        {/* BUTTONS */}
                        <div className="blockActions">
                            <button 
                                className="btn btn--secondary" 
                                type="button" 
                                onClick={() => addDescriptionBlock(selectedSubjectIndex)}
                            >
                                Tilføj fagbeskrivelse
                            </button>

                            <button 
                                className="btn btn--secondary" 
                                type="button" 
                                disabled={selectedBlocks.length === 0}
                                onClick={deleteSelectedBlocks}
                            >
                                Slet markerede fagbeskrivelse
                            </button>
                        </div>

                            <label className="label">Elevudtalelse fra læreren</label>
                            <textarea className="input" 
                                rows={6} 
                                placeholder="Elevudtalelse..."
                                value={selectedSubject?.studentStatement ?? ""}
                                onChange={(e) => {
                                    updateSubject(selectedSubjectIndex, s => ({
                                        ...s, 
                                        studentStatement: e.target.value
                                    })) 
                                }}
                            />
                        </div>
                </section>
            </section>
        </main>
    );
}