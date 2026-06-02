using SibersDataManager.Models.Projects.Dto;
using SibersDataManager.Models.Tasks.Dto;

namespace SibersDataManager.Models.Employees.Dto;

public record EmployeeToResponse(
    Guid Id, 
    string FirstName, 
    string LastName, 
    string Patronymic, 
    string Email,
    List<ProjectToResponse>? Projects,
    List<ProjectTaskToResponse>? Tasks
    );