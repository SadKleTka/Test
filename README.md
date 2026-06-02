# Sibers Test Work

A small project-management system built with ASP.NET Core. The application exposes a REST API for managing projects, employees, project tasks, and project-employee assignments. It also contains a simple static web wizard for creating projects from the browser.

## Contents

- [Overview](#overview)
- [Tech Stack](#tech-stack)
- [Architecture](#architecture)
- [Requirements](#requirements)
- [Environment Variables](#environment-variables)
- [Getting Started](#getting-started)
- [API Documentation](#api-documentation)
- [Running Tests](#running-tests)
- [Project Structure](#project-structure)
- [Logging](#logging)
- [Troubleshooting](#troubleshooting)

## Overview

The system is designed to manage:

- employees;
- projects;
- project tasks;
- project managers;
- employee assignment to projects;
- task authors and task workers;
- project and task filtering/sorting.

The backend is split into three main layers:

1. **API layer** — ASP.NET Core controllers, middleware, Swagger, static files.
2. **Service layer** — business logic and DTO mapping.
3. **Data layer** — Entity Framework Core DbContext, repositories, PostgreSQL migrations, entities.

The repository also includes MSTest-based unit tests for the main service classes.

## Tech Stack

- **.NET / ASP.NET Core** targeting `net10.0`
- **Entity Framework Core**
- **PostgreSQL** via `Npgsql.EntityFrameworkCore.PostgreSQL`
- **Docker Compose** for local PostgreSQL and Seq infrastructure
- **Serilog** for structured logging
- **Seq** for log visualization
- **Swagger / OpenAPI** for API exploration
- **MSTest** and **Moq** for tests
- Static frontend served from `wwwroot`

## Architecture

```text
Client / Browser
      |
      v
SibersTestWork.API
  - Controllers
  - Exception middleware
  - Swagger
  - Static frontend
      |
      v
SibersServices
  - ProjectService
  - EmployeeService
  - TaskService
  - ProjectEmployeeService
      |
      v
SibersDataManager
  - AppDbContext
  - EF Core repositories
  - Entities and DTOs
  - PostgreSQL migrations
      |
      v
PostgreSQL
```

### Main domain entities

#### Project

A project contains a name, customer company, worker company, start date, optional end date, priority, optional manager, employees, and tasks.

#### Employee

An employee contains personal data and can be assigned to projects, manage projects, author tasks, and work on tasks.

#### Project Task

A task belongs to a project, has an author, optional worker, status, comment, and priority.

#### Project-Employee link

Projects and employees are connected through a many-to-many relationship.

## Requirements

Install the following tools before running the project:

- .NET SDK that supports `net10.0`
- Docker and Docker Compose
- EF Core CLI tool

Install or update the EF Core CLI tool:

```bash
dotnet tool install --global dotnet-ef
# or, if already installed:
dotnet tool update --global dotnet-ef
```

## Environment Variables

The API reads the database connection string from an environment variable named `DB_CONNECTION_STRING`.

For local development, create or keep the `.env` file inside the `SibersTestWork.API` directory:

```env
DB_CONNECTION_STRING=Host=localhost;Port=5440;Database=sibers.db;Username=admin;Password=admin123
```

These credentials are intended for local development only. Do not use them in production.

## Getting Started

### 1. Clone the repository

```bash
git clone <repository-url>
cd Test-main
```

### 2. Start infrastructure services

The `compose.yaml` file starts PostgreSQL and Seq:

```bash
docker compose up -d
```

This starts:

| Service | Local URL / Port | Purpose |
|---|---:|---|
| PostgreSQL | `localhost:5440` | Application database |
| Seq | `http://localhost:5343` | Log viewer |

### 3. Restore dependencies

From the repository root:

```bash
dotnet restore SibersTestWork.sln
```

### 4. Apply database migrations

Run the migration command from the `SibersTestWork.API` directory so that `DotNetEnv` can load the local `.env` file correctly:

```bash
cd SibersTestWork.API
dotnet ef database update --project ../SibersDataManager --startup-project .
```

If you prefer running the command from the repository root, export `DB_CONNECTION_STRING` manually first.

### 5. Run the API

From the `SibersTestWork.API` directory:

```bash
dotnet run --launch-profile http
```

The API will be available at:

```text
http://localhost:5081
```

Swagger UI is available in development mode at:

```text
http://localhost:5081/swagger
```

### 6. Open the static frontend

The API serves static files from `wwwroot`. After starting the API, open:

```text
http://localhost:5081/index.html
```

The current frontend contains a basic project creation wizard.

## API Documentation

Swagger is the preferred way to inspect and test endpoints during development:

```text
http://localhost:5081/swagger
```

### Employee endpoints

| Method | Endpoint | Description |
|---|---|---|
| `GET` | `/api/Employee` | Get all employees |
| `GET` | `/api/Employee/{id}` | Get employee by ID |
| `POST` | `/api/Employee` | Create employee |
| `PUT` | `/api/Employee/{id}` | Update employee |
| `DELETE` | `/api/Employee/{id}` | Delete employee |

Example create request:

```json
{
  "firstName": "John",
  "secondName": "Smith",
  "thirdName": "Michael",
  "email": "john.smith@example.com"
}
```

### Project endpoints

| Method | Endpoint | Description |
|---|---|---|
| `GET` | `/api/Project` | Get projects with optional filtering/sorting |
| `GET` | `/api/Project/{id}` | Get project by ID |
| `POST` | `/api/Project` | Create project |
| `PUT` | `/api/Project/{id}` | Update project |
| `DELETE` | `/api/Project/{id}` | Delete project |

Supported query parameters for `GET /api/Project`:

| Parameter | Description |
|---|---|
| `priority` | Filter projects by priority |
| `sortBy` | Sort by `name`, `priority`, or `startDate` |

Example create request:

```json
{
  "name": "Internal CRM",
  "customerCompany": "Customer LLC",
  "workerCompany": "Sibers",
  "startDate": "2026-06-01T00:00:00Z",
  "priority": 5,
  "endDate": null,
  "managerId": null
}
```

### Task endpoints

| Method | Endpoint | Description |
|---|---|---|
| `GET` | `/api/Task/{id}` | Get task by ID |
| `GET` | `/api/Task/all` | Get tasks with optional filtering/sorting |
| `POST` | `/api/Task` | Create task |
| `PUT` | `/api/Task/{id}` | Update task |
| `DELETE` | `/api/Task/{id}` | Delete task |

Supported query parameters for `GET /api/Task/all`:

| Parameter | Description |
|---|---|
| `status` | Filter by task status |
| `sortBy` | Sort by `name` or `priority` |

Task status enum:

| Value | Name |
|---:|---|
| `0` | `ToDo` |
| `1` | `InProgress` |
| `2` | `Done` |

Example create request:

```json
{
  "name": "Create database schema",
  "authorId": "00000000-0000-0000-0000-000000000000",
  "workerId": null,
  "status": 0,
  "comment": "Initial database structure",
  "priority": 5,
  "projectId": "00000000-0000-0000-0000-000000000000"
}
```

### Project-Employee endpoints

| Method | Endpoint | Description |
|---|---|---|
| `POST` | `/api/ProjectEmployee?projectId={projectId}&employeeId={employeeId}` | Link employee to project |
| `POST` | `/api/ProjectEmployee/unlink?projectId={projectId}&employeeId={employeeId}` | Unlink employee from project |

## Response Format

Successful create, update, delete, link, and unlink operations return a message object similar to:

```json
{
  "messageToAnswer": "Project created successfully",
  "time": "2026-06-02T12:00:00Z"
}
```

Errors are handled by custom middleware and returned in JSON format:

```json
{
  "message": "Error message",
  "Code": 400,
  "DateTime": "2026-06-02T12:00:00Z"
}
```

## Running Tests

From the repository root:

```bash
dotnet test SibersTestWork.sln
```

The tests are located in `ProjectTests` and cover service-level behavior for:

- project creation and updates;
- employee creation and updates;
- task creation and updates;
- project-employee linking validation.

## Project Structure

```text
.
├── compose.yaml
├── SibersTestWork.sln
├── SibersTestWork.API/
│   ├── Controllers/
│   ├── MiddleWare/
│   ├── Properties/
│   ├── wwwroot/
│   ├── Program.cs
│   ├── appsettings.json
│   └── SibersTestWork.API.csproj
├── SibersDataManager/
│   ├── Data/
│   ├── Migrations/
│   ├── Models/
│   ├── Repository/
│   ├── DataBaseInjection.cs
│   └── SibersDataManager.csproj
├── SibersServices/
│   ├── Services/
│   ├── ServiceInjection.cs
│   └── SibersServices.csproj
└── ProjectTests/
    ├── EmployeeTest.cs
    ├── ProjectTest.cs
    ├── ProjectEmployeeTest.cs
    ├── TaskTest.cs
    └── ProjectTests.csproj
```

## Logging

The project uses Serilog. Logs are written to the console and can also be sent to Seq.

The Docker Compose file exposes Seq at:

```text
http://localhost:5343
```

If Seq does not receive logs, make sure the Seq URL in `SibersTestWork.API/appsettings.json` matches the Docker Compose port. For example:

```json
{
  "Serilog": {
    "WriteTo": [
      {
        "Name": "Seq",
        "Args": {
          "serverUrl": "http://localhost:5343"
        }
      }
    ]
  }
}
```

## Troubleshooting

### The API cannot connect to PostgreSQL

Check that the database container is running:

```bash
docker compose ps
```

Check that the connection string uses the mapped local port:

```env
DB_CONNECTION_STRING=Host=localhost;Port=5440;Database=sibers.db;Username=admin;Password=admin123
```

### `.env` is not loaded

Run the application from the `SibersTestWork.API` directory:

```bash
cd SibersTestWork.API
dotnet run --launch-profile http
```

Alternatively, set `DB_CONNECTION_STRING` manually in your shell before running the project.

### Swagger is not available

Swagger is enabled only when `ASPNETCORE_ENVIRONMENT` is set to `Development`. Use the provided launch profile:

```bash
dotnet run --launch-profile http
```

### Project-Employee endpoint fails with dependency injection error

If `/api/ProjectEmployee` returns a dependency injection error, make sure the project-employee service and repository are registered in the dependency injection setup:

```csharp
// SibersServices/ServiceInjection.cs
services.AddScoped<IProjectEmployeeService, ProjectEmployeeService>();

// SibersDataManager/DataBaseInjection.cs
services.AddScoped<IProjectEmployeeRepository, ProjectEmployeeRepository>();
```
