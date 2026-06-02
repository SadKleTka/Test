import { getEmployees, createProject, linkEmployee, getProjects } from "./api.js";

let step = 1;
let employees = [];
let selectedEmployees = new Set();
let createdProjectId = null;

export async function initWizard() {
    employees = await getEmployees();
    renderEmployees();
    showStep(1);
}

// ---------------- STEPS ----------------

export function nextStep() {
    if (step < 5) showStep(++step);
}

export function prevStep() {
    if (step > 1) showStep(--step);
}

function showStep(s) {
    document.querySelectorAll(".step").forEach(el => el.classList.add("hidden"));
    document.getElementById(`step-${s}`).classList.remove("hidden");
    step = s;
}

// ---------------- EMPLOYEES ----------------

function renderEmployees() {
    const manager = document.getElementById("managerSelect");
    const list = document.getElementById("employeesList");

    manager.innerHTML = "";
    list.innerHTML = "";

    employees.forEach(e => {
        const opt = document.createElement("option");
        opt.value = e.id;
        opt.innerText = `${e.firstName} ${e.secondName}`;
        manager.appendChild(opt);

        const div = document.createElement("div");
        div.className = "employee-item";

        const cb = document.createElement("input");
        cb.type = "checkbox";

        cb.onchange = () => {
            if (cb.checked) selectedEmployees.add(e.id);
            else selectedEmployees.delete(e.id);
        };

        div.appendChild(cb);
        div.appendChild(document.createTextNode(`${e.firstName} ${e.secondName}`));
        list.appendChild(div);
    });
}

// ---------------- CREATE FLOW ----------------

export async function submitProject() {

    const dto = {
        name: document.getElementById("projectName").value,
        customerCompany: document.getElementById("customerCompany").value,
        workerCompany: document.getElementById("workerCompany").value,
        startDate: document.getElementById("startDate").value,
        endDate: document.getElementById("endDate").value || null,
        priority: Number(document.getElementById("priority").value),
        managerId: document.getElementById("managerSelect").value || null
    };

    // 1. CREATE PROJECT
    const project = await createProject(dto);

    createdProjectId = project.id || project;

    // 2. LINK EMPLOYEES
    for (const empId of selectedEmployees) {
        await linkEmployee(createdProjectId, empId);
    }

    alert("Project created successfully!");
}