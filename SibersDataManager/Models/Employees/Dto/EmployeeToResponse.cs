namespace SibersDataManager.Models.Employees.Dto;

public record EmployeeToResponse(
    Guid Id, 
    string FirstName, 
    string LastName, 
    string Patronymic, 
    string Email);