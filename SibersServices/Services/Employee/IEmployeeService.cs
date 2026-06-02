using SibersDataManager.Models;
using SibersDataManager.Models.Employees.Dto;

namespace SibersServices.Services.Employee;

public interface IEmployeeService
{
    Task<EmployeeToResponse> GetById(Guid id);
    Task<IEnumerable<EmployeeToResponse>> GetAll(string? search = null);
    Task<Message> CreateEmployee(EmployeeToCreate dto);
    Task<Message> UpdateEmployee(Guid id, EmployeeToCreate dto);
    Task<Message> DeleteEmployeeById(Guid id);
}