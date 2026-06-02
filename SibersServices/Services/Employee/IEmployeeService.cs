using SibersDataManager.Models;
using SibersDataManager.Models.Employees.Dto;

namespace SibersServices.Services.Employee;

public interface IEmployeeService
{
    public Task<EmployeeToResponse> GetById(Guid id);

    public Task<IEnumerable<EmployeeToResponse>> GetAll();

    public Task<Message> CreateEmployee(EmployeeToCreate dto);

    public Task<Message> UpdateEmployee(Guid id, EmployeeToCreate dto);

    public Task<Message> DeleteEmployeeById(Guid id);

}