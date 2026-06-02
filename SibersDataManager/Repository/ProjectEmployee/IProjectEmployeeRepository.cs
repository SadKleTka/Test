using SibersDataManager.Models.Employees;
using SibersDataManager.Models.Projects;

namespace SibersDataManager.Repository.ProjectEmployee;

public interface IProjectEmployeeRepository
{
    public Task LinkAsync(Guid projectId, Guid employeeId);

    public Task UnLinkAsync(Guid projectId, Guid employeeId);

    public Task<ProjectEntity> GetAllOfProject(Guid projectId);

    public Task<EmployeeEntity> GetEmployeeAsync(Guid employeeId);
}