using Moq;
using SibersDataManager.Models.Employees;
using SibersDataManager.Models.Exceptions;
using SibersDataManager.Models.Projects;
using SibersDataManager.Repository.ProjectEmployee;
using SibersServices.Services.ProjectEmployee;

namespace ProjectTests;

[TestClass]
public class ProjectEmployeeTest
{
    private IProjectEmployeeService _service;
    private Mock<IProjectEmployeeRepository> _repository;

    [TestInitialize]
    public void Setup()
    {
        _repository = new Mock<IProjectEmployeeRepository>();
        _service = new ProjectEmployeeService(_repository.Object);
    }
    [TestMethod]
    public async Task LinkProjectEmployee_LinkSuccess_ReturnsApproveMessage()
    {
        var employeeId = Guid.NewGuid();
        var projectId = Guid.NewGuid();

        var employee = new EmployeeEntity(
            "Test",
            "Test", 
            "Test,", 
            "test@sibers.com");
        employee.Id = employeeId; 
    
        var project = new ProjectEntity
        {
            Id = projectId,
            Name = "Test Project",
        };
        
        _repository.Setup(r => r.GetAllOfProject(projectId))
            .ReturnsAsync(project);
        _repository.Setup(r => r.GetEmployeeAsync(employeeId))
            .ReturnsAsync(employee);

        var result = await _service.LinkEmployeeAndProject(projectId, employeeId);
        
        Assert.IsNotNull(result);
        Assert.AreEqual("Successfully linked project with employee", result.MessageToAnswer);
    }
    
    
    [TestMethod]
    public async Task LinkProjectEmployee_ProjectAlreadyHasEmployee_ThrowsBusinessValidationException()
    {
        var employeeId = Guid.NewGuid();
        var projectId = Guid.NewGuid();

        var employee = new EmployeeEntity(
            "Test",
            "Test", 
            "Test,", 
            "test@sibers.com");
        employee.Id = employeeId; 
    
        var project = new ProjectEntity
        {
            Id = projectId,
            Name = "Test Project",
        };
        
        project.Employees = new List<EmployeeEntity> { employee };
        
        _repository.Setup(r => r.GetAllOfProject(projectId))
            .ReturnsAsync(project);
        _repository.Setup(r => r.GetEmployeeAsync(employeeId))
            .ReturnsAsync(employee);

        await Assert.ThrowsAsync<BusinessValidationException>(() => 
             _service.LinkEmployeeAndProject(projectId, employeeId));
    }
    
}