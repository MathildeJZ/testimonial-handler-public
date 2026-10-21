
export async function getByStudentId(id) {
    const response = await fetch(`/api/students/${id}`);

    if (!response.ok) {
        throw new Error("Failed to fetch student");
    }

    return await response.json();
}