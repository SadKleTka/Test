using Moq;
using SibersDataManager.Models.Exceptions;
using SibersDataManager.Models.Tasks;
using SibersDataManager.Models.Roles;
using SibersDataManager.Models.Tasks.Dto;
using SibersDataManager.Repository.ProjectTask;
using SibersServices.Services.Task;

namespace ProjectTests;

[TestClass]
public class TaskTest
{
    private TaskService _service;
    private Mock<IProjectTaskRepository> _repositoryMock;

    [TestInitialize]
    public void Setup()
    {
        _repositoryMock = new Mock<IProjectTaskRepository>();
        _service = new TaskService(_repositoryMock.Object);
    }

    [TestMethod]
    public async Task CreateTask_PostSuccess_ReturnSuccessMessage()
    {
        var taskDto = new ProjectTaskToCreate(
            "Test", 
            Guid.NewGuid(), 
            Guid.NewGuid(), 
            TaskStatusEnum.ToDo, 
            "Testing", 
            5, 
            Guid.NewGuid());

        var result = await _service.CreateTask(taskDto);

        Assert.AreEqual("Task created successfully", result.MessageToAnswer);
    }

    [TestMethod]
    public async Task UpdateTask_UpdateSuccess_ReturnSuccessMessage()
    {
        var taskId = Guid.NewGuid();
        var existingEntity = new ProjectTaskEntity(
            "Old Name", 
            Guid.NewGuid(), 
            Guid.NewGuid(),
            TaskStatusEnum.Done, 
            "Comment", 
            1, 
            Guid.NewGuid())
        {
            Id = taskId
        };

        var updateDto = new ProjectTaskToUpdate(
            "New Name", 
            Guid.NewGuid(), 
            TaskStatusEnum.InProgress, 
            "New Comment", 
            3);

        _repositoryMock.Setup(u => u.FindAsync(taskId)).ReturnsAsync(existingEntity);

        var result = await _service.UpdateTask(taskId, updateDto);

        Assert.AreEqual("Task updated successfully", result.MessageToAnswer);
    }

    [TestMethod]
    public async Task UpdateTask_EntityIsNull_ThrowsNotFoundException()
    {
        var taskId = Guid.NewGuid();
        
        var updateDto = new ProjectTaskToUpdate(
            "Name", 
            Guid.NewGuid(),
            TaskStatusEnum.Done, 
            "Comment", 
            5);
        
        await Assert.ThrowsAsync<NotFoundException>(() => _service.UpdateTask(taskId, updateDto));
    }
}