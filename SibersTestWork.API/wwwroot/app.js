const API_BASE = `${window.location.origin}/api`;

const WIZARD_STEPS = 5;
const state = {
    currentStep: 1,
    manager: null,
    executors: new Map(),
    documents: []
};

const elements = {};

document.addEventListener("DOMContentLoaded", () => {
    cacheElements();
    bindEvents();
    setupEmployeeSearch({
        input: elements.managerSearch,
        dropdown: elements.managerDropdown,
        onSelect: selectManager,
        emptyText: "Сотрудники не найдены"
    });
    setupEmployeeSearch({
        input: elements.executorSearch,
        dropdown: elements.executorDropdown,
        onSelect: addExecutor,
        emptyText: "Сотрудники не найдены"
    });
    render();
});

function cacheElements() {
    elements.form = document.getElementById("projectWizard");
    elements.alert = document.getElementById("formAlert");
    elements.stepCounter = document.getElementById("stepCounter");
    elements.progress = document.getElementById("progress");

    elements.prevButton = document.getElementById("prevButton");
    elements.nextButton = document.getElementById("nextButton");
    elements.submitButton = document.getElementById("submitButton");

    elements.projectName = document.getElementById("projectName");
    elements.startDate = document.getElementById("startDate");
    elements.endDate = document.getElementById("endDate");
    elements.priority = document.getElementById("priority");
    elements.customerCompany = document.getElementById("customerCompany");
    elements.workerCompany = document.getElementById("workerCompany");

    elements.managerSearch = document.getElementById("managerSearch");
    elements.managerDropdown = document.getElementById("managerDropdown");
    elements.managerId = document.getElementById("managerId");
    elements.managerSelected = document.getElementById("managerSelected");

    elements.executorSearch = document.getElementById("executorSearch");
    elements.executorDropdown = document.getElementById("executorDropdown");
    elements.selectedExecutors = document.getElementById("selectedExecutors");

    elements.documents = document.getElementById("documents");
    elements.fileList = document.getElementById("fileList");
}

function bindEvents() {
    elements.prevButton.addEventListener("click", previousStep);
    elements.nextButton.addEventListener("click", nextStep);
    elements.form.addEventListener("submit", submitWizard);
    elements.documents.addEventListener("change", handleDocumentsChange);

    document.addEventListener("click", (event) => {
        if (!event.target.closest(".combo")) {
            hideDropdown(elements.managerDropdown, elements.managerSearch);
            hideDropdown(elements.executorDropdown, elements.executorSearch);
        }
    });
}

function render() {
    document.querySelectorAll(".step").forEach((stepElement) => {
        stepElement.classList.toggle("hidden", Number(stepElement.dataset.step) !== state.currentStep);
    });

    elements.stepCounter.textContent = `Шаг ${state.currentStep} из ${WIZARD_STEPS}`;

    [...elements.progress.children].forEach((item, index) => {
        const stepNumber = index + 1;
        item.classList.toggle("active", stepNumber === state.currentStep);
        item.classList.toggle("done", stepNumber < state.currentStep);
    });

    elements.prevButton.classList.toggle("hidden", state.currentStep === 1);
    elements.nextButton.classList.toggle("hidden", state.currentStep === WIZARD_STEPS);
    elements.submitButton.classList.toggle("hidden", state.currentStep !== WIZARD_STEPS);

    renderManager();
    renderExecutors();
    renderFiles();
}

function nextStep() {
    clearAlert();

    const error = validateStep(state.currentStep);
    if (error) {
        showAlert(error);
        return;
    }

    state.currentStep = Math.min(WIZARD_STEPS, state.currentStep + 1);
    render();
}

function previousStep() {
    clearAlert();
    state.currentStep = Math.max(1, state.currentStep - 1);
    render();
}

function validateStep(step) {
    if (step === 1) {
        const name = elements.projectName.value.trim();
        const startDate = elements.startDate.value;
        const endDate = elements.endDate.value;
        const priority = Number(elements.priority.value);

        if (!name) return "Введите название проекта.";
        if (!startDate) return "Укажите дату начала проекта.";
        if (endDate && endDate < startDate) return "Дата окончания не может быть раньше даты начала.";
        if (!Number.isInteger(priority) || priority < 1 || priority > 10) return "Выберите приоритет от 1 до 10.";
    }

    if (step === 2) {
        if (!elements.customerCompany.value.trim()) return "Введите компанию-заказчика.";
        if (!elements.workerCompany.value.trim()) return "Введите компанию-исполнителя.";
    }

    if (step === 3 && !state.manager) {
        return "Выберите руководителя проекта из списка сотрудников.";
    }

    if (step === 4 && state.executors.size === 0) {
        return "Выберите хотя бы одного исполнителя проекта.";
    }

    return null;
}

function setupEmployeeSearch({ input, dropdown, onSelect, emptyText }) {
    const debouncedSearch = debounce(async () => {
        await renderEmployeeDropdown(input, dropdown, onSelect, emptyText);
    }, 300);

    input.addEventListener("focus", () => renderEmployeeDropdown(input, dropdown, onSelect, emptyText));
    input.addEventListener("input", debouncedSearch);
    input.addEventListener("keydown", (event) => {
        if (event.key === "Escape") hideDropdown(dropdown, input);
    });
}

async function renderEmployeeDropdown(input, dropdown, onSelect, emptyText) {
    const query = input.value.trim();
    showDropdownState(dropdown, "Ищем сотрудников...", input);

    try {
        const employees = await fetchEmployees(query);
        const filteredEmployees = employees.filter((employee) => !state.executors.has(getEmployeeId(employee)) || input.id !== "executorSearch");

        dropdown.innerHTML = "";

        if (filteredEmployees.length === 0) {
            showDropdownState(dropdown, emptyText, input);
            return;
        }

        filteredEmployees.forEach((employee) => {
            const button = document.createElement("button");
            button.type = "button";
            button.className = "dropdown-item";
            button.setAttribute("role", "option");
            button.innerHTML = `
                <strong>${escapeHtml(getEmployeeName(employee))}</strong>
                <small>${escapeHtml(getEmployeeEmail(employee))}</small>
            `;
            button.addEventListener("click", () => {
                onSelect(employee);
                hideDropdown(dropdown, input);
            });
            dropdown.appendChild(button);
        });

        showDropdown(dropdown, input);
    } catch (error) {
        console.error(error);
        showDropdownState(dropdown, "Не удалось загрузить сотрудников", input);
    }
}

async function fetchEmployees(search) {
    const url = new URL(`${API_BASE}/employee`);
    if (search) url.searchParams.set("search", search);

    const response = await fetch(url, { headers: { "Accept": "application/json" } });
    if (!response.ok) throw new Error("Employee search failed");

    const data = await response.json();
    return Array.isArray(data) ? data : [];
}

function selectManager(employee) {
    state.manager = normalizeEmployee(employee);
    elements.managerId.value = state.manager.id;
    elements.managerSearch.value = "";
    clearAlert();
    renderManager();
}

function addExecutor(employee) {
    const normalizedEmployee = normalizeEmployee(employee);
    state.executors.set(normalizedEmployee.id, normalizedEmployee);
    elements.executorSearch.value = "";
    clearAlert();
    renderExecutors();
}

function renderManager() {
    if (!state.manager) {
        elements.managerSelected.classList.add("empty");
        elements.managerSelected.textContent = "Руководитель не выбран";
        return;
    }

    elements.managerSelected.classList.remove("empty");
    elements.managerSelected.textContent = `${state.manager.name} · ${state.manager.email}`;
}

function renderExecutors() {
    elements.selectedExecutors.innerHTML = "";

    if (state.executors.size === 0) {
        const empty = document.createElement("div");
        empty.className = "selected-box empty";
        empty.textContent = "Исполнители пока не выбраны";
        elements.selectedExecutors.appendChild(empty);
        return;
    }

    state.executors.forEach((employee) => {
        const chip = document.createElement("div");
        chip.className = "chip";
        chip.innerHTML = `<span>${escapeHtml(employee.name)}</span>`;

        const removeButton = document.createElement("button");
        removeButton.type = "button";
        removeButton.title = "Убрать исполнителя";
        removeButton.textContent = "×";
        removeButton.addEventListener("click", () => {
            state.executors.delete(employee.id);
            renderExecutors();
        });

        chip.appendChild(removeButton);
        elements.selectedExecutors.appendChild(chip);
    });
}

function handleDocumentsChange(event) {
    state.documents = [...event.target.files];
    renderFiles();
}

function renderFiles() {
    elements.fileList.innerHTML = "";

    if (state.documents.length === 0) return;

    state.documents.forEach((file) => {
        const item = document.createElement("li");
        item.innerHTML = `
            <span>${escapeHtml(file.name)}</span>
            <small>${formatFileSize(file.size)}</small>
        `;
        elements.fileList.appendChild(item);
    });
}

async function submitWizard(event) {
    event.preventDefault();
    clearAlert();

    const error = validateStep(state.currentStep);
    if (error) {
        showAlert(error);
        return;
    }

    setSubmitting(true);

    try {
        const project = await createProject();
        const projectId = getProjectId(project);

        if (!projectId) {
            throw new Error("Проект создан, но API не вернул id проекта. Проверьте метод POST /api/project/with-id.");
        }

        await linkExecutors(projectId);
        await uploadDocuments(projectId);

        showAlert("Проект создан, исполнители привязаны, документы загружены.", "success");
        elements.form.reset();
        state.currentStep = 1;
        state.manager = null;
        state.executors.clear();
        state.documents = [];
        render();
    } catch (error) {
        console.error(error);
        showAlert(error.message || "Не удалось создать проект. Проверьте API и заполненные данные.");
    } finally {
        setSubmitting(false);
    }
}

async function createProject() {
    const dto = collectProjectDto();
    let response = await fetch(`${API_BASE}/project/with-id`, {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
            "Accept": "application/json"
        },
        body: JSON.stringify(dto)
    });

    if (response.status === 404 || response.status === 405) {
        response = await fetch(`${API_BASE}/project`, {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                "Accept": "application/json"
            },
            body: JSON.stringify(dto)
        });
    }

    const payload = await readJson(response);
    if (!response.ok) throw new Error(getApiError(payload, "Не удалось создать проект."));

    if (getProjectId(payload)) return payload;

    const resolvedProject = await findCreatedProject(dto);
    return resolvedProject || payload;
}

function collectProjectDto() {
    return {
        name: elements.projectName.value.trim(),
        customerCompany: elements.customerCompany.value.trim(),
        workerCompany: elements.workerCompany.value.trim(),
        startDate: toApiDate(elements.startDate.value),
        priority: Number(elements.priority.value),
        endDate: elements.endDate.value ? toApiDate(elements.endDate.value) : null,
        managerId: state.manager?.id || null
    };
}

async function findCreatedProject(dto) {
    const response = await fetch(`${API_BASE}/project`, { headers: { "Accept": "application/json" } });
    if (!response.ok) return null;

    const projects = await response.json();
    if (!Array.isArray(projects)) return null;

    return projects
        .filter((project) =>
            normalizeText(project.name) === normalizeText(dto.name) &&
            normalizeText(project.customerCompany) === normalizeText(dto.customerCompany) &&
            normalizeText(project.workerCompany) === normalizeText(dto.workerCompany))
        .sort((a, b) => new Date(b.startDate) - new Date(a.startDate))[0] || null;
}

async function linkExecutors(projectId) {
    const requests = [...state.executors.keys()].map((employeeId) => {
        const url = new URL(`${API_BASE}/ProjectEmployee`);
        url.searchParams.set("projectId", projectId);
        url.searchParams.set("employeeId", employeeId);
        return fetch(url, { method: "POST", headers: { "Accept": "application/json" } });
    });

    const responses = await Promise.all(requests);
    const failed = responses.find((response) => !response.ok);
    if (failed) {
        const payload = await readJson(failed);
        throw new Error(getApiError(payload, "Проект создан, но не удалось привязать одного или нескольких исполнителей."));
    }
}

async function uploadDocuments(projectId) {
    if (state.documents.length === 0) return;

    const formData = new FormData();
    state.documents.forEach((file) => formData.append("files", file));

    const response = await fetch(`${API_BASE}/project/${projectId}/documents`, {
        method: "POST",
        body: formData
    });

    const payload = await readJson(response);
    if (!response.ok) throw new Error(getApiError(payload, "Проект создан, но документы не загрузились."));
}

function normalizeEmployee(employee) {
    return {
        id: getEmployeeId(employee),
        name: getEmployeeName(employee),
        email: getEmployeeEmail(employee)
    };
}

function getEmployeeId(employee) {
    return employee.id || employee.Id;
}

function getEmployeeName(employee) {
    const firstName = employee.firstName || employee.name || employee.Name || "";
    const lastName = employee.lastName || employee.secondName || employee.SecondName || "";
    const patronymic = employee.patronymic || employee.thirdName || employee.ThirdName || "";
    const fullName = [lastName, firstName, patronymic].filter(Boolean).join(" ").trim();
    return fullName || employee.email || employee.Email || "Сотрудник без имени";
}

function getEmployeeEmail(employee) {
    return employee.email || employee.Email || "email не указан";
}

function getProjectId(project) {
    return project?.id || project?.Id || project?.projectId || project?.ProjectId || null;
}

function showDropdown(dropdown, input) {
    dropdown.classList.remove("hidden");
    input.setAttribute("aria-expanded", "true");
}

function hideDropdown(dropdown, input) {
    dropdown.classList.add("hidden");
    input.setAttribute("aria-expanded", "false");
}

function showDropdownState(dropdown, text, input) {
    dropdown.innerHTML = `<div class="dropdown-state">${escapeHtml(text)}</div>`;
    showDropdown(dropdown, input);
}

function showAlert(message, type = "error") {
    elements.alert.textContent = message;
    elements.alert.classList.remove("hidden", "success");
    elements.alert.classList.toggle("success", type === "success");
}

function clearAlert() {
    elements.alert.textContent = "";
    elements.alert.classList.add("hidden");
    elements.alert.classList.remove("success");
}

function setSubmitting(isSubmitting) {
    elements.submitButton.disabled = isSubmitting;
    elements.nextButton.disabled = isSubmitting;
    elements.prevButton.disabled = isSubmitting;
    elements.submitButton.textContent = isSubmitting ? "Создаём..." : "Создать проект";
}

function toApiDate(value) {
    return new Date(`${value}T00:00:00.000Z`).toISOString();
}

async function readJson(response) {
    const text = await response.text();
    if (!text) return null;

    try {
        return JSON.parse(text);
    } catch {
        return text;
    }
}

function getApiError(payload, fallback) {
    if (!payload) return fallback;
    if (typeof payload === "string") return payload;
    if (payload.messageToAnswer) return payload.messageToAnswer;
    if (payload.message) return payload.message;
    if (payload.title) return payload.title;
    if (payload.errors) return Object.values(payload.errors).flat().join(" ");
    return fallback;
}

function formatFileSize(size) {
    if (size < 1024) return `${size} Б`;
    if (size < 1024 * 1024) return `${(size / 1024).toFixed(1)} КБ`;
    return `${(size / 1024 / 1024).toFixed(1)} МБ`;
}

function normalizeText(value) {
    return String(value || "").trim().toLowerCase();
}

function debounce(callback, delay) {
    let timer;
    return (...args) => {
        clearTimeout(timer);
        timer = setTimeout(() => callback(...args), delay);
    };
}

function escapeHtml(value) {
    return String(value ?? "")
        .replaceAll("&", "&amp;")
        .replaceAll("<", "&lt;")
        .replaceAll(">", "&gt;")
        .replaceAll('"', "&quot;")
        .replaceAll("'", "&#039;");
}
