const API_URL = `${window.location.origin}/api`;

export async function getEmployees(search = "") {
    const url = new URL(`${API_URL}/employee`);
    if (search.trim()) url.searchParams.set("search", search.trim());

    const res = await fetch(url, { headers: { "Accept": "application/json" } });
    return await res.json();
}

export async function createProject(data) {
    const res = await fetch(`${API_URL}/project/with-id`, {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
            "Accept": "application/json"
        },
        body: JSON.stringify(data)
    });
    return await res.json();
}

export async function linkEmployee(projectId, employeeId) {
    const res = await fetch(`${API_URL}/ProjectEmployee?projectId=${projectId}&employeeId=${employeeId}`, {
        method: "POST",
        headers: { "Accept": "application/json" }
    });
    return await res.json();
}

export async function uploadProjectDocuments(projectId, files) {
    const formData = new FormData();
    [...files].forEach((file) => formData.append("files", file));

    const res = await fetch(`${API_URL}/project/${projectId}/documents`, {
        method: "POST",
        body: formData
    });
    return await res.json();
}

export async function getProjects() {
    const res = await fetch(`${API_URL}/project`, { headers: { "Accept": "application/json" } });
    return await res.json();
}
