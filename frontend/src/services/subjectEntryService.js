export async function createSubjectEntry(payload) {
    console.log("🔥 CORRECT FUNCTION IS RUNNING");
    const token = localStorage.getItem("token");

    console.log("🔥 ABOUT TO FETCH");
    
    const response = await fetch(
        `/api/subjectEntry`,
        {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                "Authorization": `Bearer ${token}`  
            },
            body: JSON.stringify(payload)
        }
    );

    const text = await response.text();

    console.log("🔥 RESPONSE STATUS:", response.status);

    
    if(!response.ok){
        console.error("💀ERROR RESPONSE:", text);
        throw new Error("🫧Fejl i at oprette fagbeskrivelse🫧", text);
    }

    return JSON.parse(text);
}