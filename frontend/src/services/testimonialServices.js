export async function createTestimonial(payload) {
    console.log("🔥 CORRECT FUNCTION IS RUNNING");
    const token = localStorage.getItem("token");

    console.log("🔥 ABOUT TO FETCH");
    
    const response = await fetch(
        `/api/testimonials`,
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

    console.log("🔥 RESPONSE BODY:", text);

    if(!response.ok){
        throw new Error(text);
    }

    return JSON.parse(text);
}