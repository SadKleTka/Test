const API_URL = "http://localhost:5081/api";

export async function getEmployees() {
    const res = await fetch(`${API_URL}/employee`);
    return await res.json();
}

export async function createProject(data) {
    const res = await fetch(`${API_URL}/project`, {
        method: "POST",
        headers: {"Content-Type": "application/json"},
        body: JSON.stringify(data)
    });
    return await res.json();
}

export async function linkEmployee(projectId, employeeId) {
    const res = await fetch(`${API_URL}/ProjectEmployee?projectId=${projectId}&employeeId=${employeeId}`, {
        method: "POST"
    });
    return await res.json();
}

export async function getProjects() {
    const res = await fetch(`${API_URL}/project`);
    return await res.json();
}