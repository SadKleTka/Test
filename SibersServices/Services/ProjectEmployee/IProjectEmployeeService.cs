using SibersDataManager.Models;

namespace SibersServices.Services.ProjectEmployee;

public interface IProjectEmployeeService
{
    public Task<Message> LinkEmployeeAndProject(Guid projectId, Guid employeeId);
    public Task<Message> UnlinkEmployeeAndProject(Guid projectId, Guid employeeId);
}