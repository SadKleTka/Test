using SibersDataManager.Models.Employees;

namespace SibersDataManager.Repository.Employee;

public interface IEmployeeRepository
{
    
    Task<EmployeeEntity> GetByEmailAsync(string email);
    Task<EmployeeEntity?> FindAsync(Guid id);
    
    Task<IEnumerable<EmployeeEntity>> GetAllAsync();
    
    Task PersistAsync(EmployeeEntity entity);
    
    Task UpdateAsync(EmployeeEntity entity);
    
    Task DeleteAsync(EmployeeEntity entity);
}