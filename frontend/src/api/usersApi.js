export async function createUser(form){
    const response = await fetch("/api/users", {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
        },
        body: JSON.stringify(form)
    });

    if (!response.ok){
        throw new Error ("Kunne ikke oprette bruger");
    }

return response.json();
}

//modtager form-data fra react, sender til backend-endpoint(/api/users),
//venter på et svar, returnere resultat fra React-siden.
//En bro mellem frontend og backend.