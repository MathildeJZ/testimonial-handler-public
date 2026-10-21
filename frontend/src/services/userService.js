const API = "/api";

export async function createUserService(form) {
  const response = await fetch(`${API}/user`, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify(form),
  });

  const contentType = response.headers.get("content-type") || "";
  const data = contentType.includes("application/json")
    ? await response.json()
    : await response.text();

  if (!response.ok) {
    const message = Array.isArray(data)
      ? data.join(",")
      : data || `HTTP ${response.status}`;
    throw new Error(message);
  }

  return data;
}

export async function getUserById(userId) {
  const response = await fetch(`${API}/user/${userId}`);

  if (!response.ok) {
    const text = await response.text();
    throw new Error(text || "Fejl ved hentning af bruger");
  }

  return await response.json();
}


    
  




