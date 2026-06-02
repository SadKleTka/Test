using Moq;
using SibersDataManager.Models.Exceptions;
using SibersDataManager.Models.Projects;
using SibersDataManager.Models.Projects.Dto;
using SibersDataManager.Repository.ProjectRepository;
using SibersServices.Services;

namespace ProjectTests;

[TestClass]
public class ProjectTest
{

    private ProjectService _service;
    private Mock<IProjectRepository> _repositoryMock;
    
    
    [TestInitialize]
    public void Setup()
    {
        _repositoryMock = new Mock<IProjectRepository>();
        _service = new ProjectService(_repositoryMock.Object);
    }
    
    
    [TestMethod]
    public async Task CreateProject_EndDateBeforeStartDate_ThrowsBusinessValidationException()
    {
        var project = new ProjectToCreate(
            "Sibers Project", 
            "Customer", 
            "Worker", 
            DateTime.UtcNow, 
            5, 
            DateTime.UtcNow.AddDays(-1), 
            null);
        
        await Assert.ThrowsAsync<BusinessValidationException>(
            () => _service.CreateProject(project)
        );
    }

    [TestMethod]
    public async Task CreateProject_PostSuccess_ReturnSuccessMessage()
    {
        var project = new ProjectToCreate(
            "Sibers Project", 
            "Customer", 
            "Worker", 
            DateTime.UtcNow, 
            5, 
            null, 
            null);
        
        var result = await _service.CreateProject(project);
        
        Assert.AreEqual("Project created successfully", result.MessageToAnswer);
    }

    [TestMethod]
    public async Task UpdateProject_UpdateSuccess_ReturnSuccessMessage()
    {
        var projectId = Guid.NewGuid();

        var existingEntity = new ProjectEntity
        {
            Id = projectId,
            Name = "Old Name",
            CustomerCompany = "Old Customer",
            WorkerCompany = "Old Worker",
            StartDate = DateTime.UtcNow.AddDays(-5),
            Priority = 3,
            EndDate = null,
            ManagerId = null
        };

        var project = new ProjectToUpdate(
            "New Name",
            "New CustomerCompany",
            "New WorkerCompany",
            5,
            null,
            null);
        
        _repositoryMock.Setup(u => u.FindAsync(projectId)).ReturnsAsync(existingEntity);
        
        var result = await _service.UpdateProject(projectId, project);
        
        Assert.AreEqual("Project updated successfully", result.MessageToAnswer);
    }

    [TestMethod]
    public async Task UpdateProject_EntityIsNull_ThrowsNotFoundException()
    {
        var projectId = Guid.NewGuid();
        
        var project = new ProjectToUpdate(
            "New Name",
            "New CustomerCompany",
            "New WorkerCompany",
            5,
            null,
            null);


        await Assert.ThrowsAsync<NotFoundException>(() => _service.UpdateProject(projectId, project));
    }
}