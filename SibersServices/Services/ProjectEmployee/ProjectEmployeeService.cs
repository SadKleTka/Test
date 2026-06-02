using SibersDataManager.Repository.ProjectEmployee;
using SibersDataManager.Models;
using SibersDataManager.Models.Exceptions;


namespace SibersServices.Services.ProjectEmployee;

public class ProjectEmployeeService : IProjectEmployeeService
{
    private readonly IProjectEmployeeRepository _repository;
    
    public ProjectEmployeeService(IProjectEmployeeRepository repository)
    {
        _repository = repository;
    }

    public async Task<Message> LinkEmployeeAndProject(Guid projectId, Guid employeeId)
    {
        var project = await _repository.GetAllOfProject(projectId);
        if (project is null)
            throw new NotFoundException("There are no project with such id.");
        
        var employee = await _repository.GetEmployeeAsync(employeeId);
        if (employee is null)
            throw new NotFoundException("There is no employee with such id.");

        if (project.Employees.Contains(employee) || employee.Projects.Contains(project))
            throw new BusinessValidationException("Employee already has been working on this project");

        await _repository.LinkAsync(projectId, employeeId);
        return new Message("Successfully linked project with employee", DateTime.UtcNow);
    }

    public async Task<Message> UnlinkEmployeeAndProject(Guid projectId, Guid employeeId)
    {
        var project = await _repository.GetAllOfProject(projectId);
        if (project is null)
            throw new NotFoundException("There are no project with such id.");
        
        var employee = await _repository.GetEmployeeAsync(employeeId);
        if (employee is null)
            throw new NotFoundException("There is no employee with such id.");
        
        if (!project.Employees.Contains(employee) || !employee.Projects.Contains(project))
            throw new BusinessValidationException("Employee is not working on this project");

        await _repository.UnLinkAsync(projectId, employeeId);
        return new Message("Successfully unlinked project with employee", DateTime.UtcNow);
    }
}