const API = "http://localhost:5081/api";

let step = 1;
let employees = [];
let selectedEmployees = new Set();

document.addEventListener("DOMContentLoaded", async () => {
    await loadEmployees();
    showStep(1);
});

// ---------------- STEPS ----------------

function showStep(s) {
    document.querySelectorAll(".step").forEach(e => e.classList.add("hidden"));
    document.getElementById(`step-${s}`).classList.remove("hidden");
    document.getElementById("stepIndicator").innerText = `Step ${s} / 5`;
    step = s;
}

function nextStep() { if (step < 5) showStep(step + 1); }
function prevStep() { if (step > 1) showStep(step - 1); }

// ---------------- EMPLOYEES ----------------

async function loadEmployees() {
    const res = await fetch(`${API}/employee`);
    employees = await res.json();

    const managerSelect = document.getElementById("managerSelect");
    const list = document.getElementById("employeesList");

    managerSelect.innerHTML = "";
    list.innerHTML = "";

    employees.forEach(e => {

        const opt = document.createElement("option");
        opt.value = e.id;
        opt.innerText = `${e.firstName} ${e.secondName}`;
        managerSelect.appendChild(opt);

        const div = document.createElement("div");
        div.className = "employee";

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

// ---------------- CREATE PROJECT (DTO FIXED) ----------------

async function submit() {

    // ⚠️ ВАЖНО: строго под ProjectToCreate
    const dto = {
        name: document.getElementById("name").value,
        customerCompany: document.getElementById("customerCompany").value,
        workerCompany: document.getElementById("workerCompany").value,
        startDate: new Date(document.getElementById("startDate").value).toISOString(),
        endDate: document.getElementById("endDate").value
            ? new Date(document.getElementById("endDate").value).toISOString()
            : null,
        priority: Number(document.getElementById("priority").value),
        managerId: document.getElementById("managerSelect").value || null
    };

    console.log("DTO SENT:", dto);

    const res = await fetch(`${API}/project`, {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify(dto)
    });

    const data = await res.json();
    console.log("RESPONSE:", data);

    // ❗ НЕ предполагаем что есть id
    alert("Project created (check backend response)");

    // если backend вернёт GUID — можно попробовать:
    const projectId = data.id ?? data?.message ?? data;

    console.log("ProjectId guess:", projectId);

    // link employees (если projectId реально GUID)
    if (typeof projectId === "string") {
        for (let empId of selectedEmployees) {
            await fetch(`${API}/ProjectEmployee?projectId=${projectId}&employeeId=${empId}`, {
                method: "POST"
            });
        }
    }
}