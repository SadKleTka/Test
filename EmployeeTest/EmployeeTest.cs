namespace EmployeeTest;

using Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SibersDataManager.Models.Employees;
using SibersDataManager.Models.Employees.Dto;
using SibersDataManager.Models.Exceptions;
using SibersDataManager.Repository.Employee;
using SibersServices.Services.Employee;

namespace ProjectTests;

[TestClass]
public class EmployeeTest
{
    private EmployeeService _service;
    private Mock<IEmployeeRepository> _repositoryMock;

    [TestInitialize]
    public void Setup()
    {
        _repositoryMock = new Mock<IEmployeeRepository>();
        _service = new EmployeeService(_repositoryMock.Object);
    }

    [TestMethod]
    public async Task CreateEmployee_PostSuccess_ReturnSuccessMessage()
    {
        // Arrange
        var employeeDto = new EmployeeToCreate(
            "Иван", 
            "Иванов", 
            "Иванович", 
            "ivan@sibers.com");

        // Act
        var result = await _service.CreateEmployee(employeeDto);

        // Assert
        Assert.AreEqual("Employee created successfully", result.MessageToAnswer);
    }

    [TestMethod]
    public async Task UpdateEmployee_UpdateSuccess_ReturnSuccessMessage()
    {
        // Arrange
        var employeeId = Guid.NewGuid();
        var existingEntity = new EmployeeEntity("Old", "Old", "Old", "old@sibers.com")
        {
            Id = employeeId
        };

        var updateDto = new EmployeeToUpdate(
            "New Name", 
            "New LastName", 
            "New Patronymic", 
            "new@sibers.com");

        _repositoryMock.Setup(u => u.FindAsync(employeeId)).ReturnsAsync(existingEntity);

        // Act
        var result = await _service.UpdateEmployee(employeeId, updateDto);

        // Assert
        Assert.AreEqual("Employee updated successfully", result.MessageToAnswer);
    }

    [TestMethod]
    public async Task UpdateEmployee_EntityIsNull_ThrowsNotFoundException()
    {
        // Arrange
        var employeeId = Guid.NewGuid();
        var updateDto = new EmployeeToUpdate("Name", "LastName", "Patronymic", "email@sibers.com");

        _repositoryMock.Setup(u => u.FindAsync(employeeId)).ReturnsAsync((EmployeeEntity)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _service.UpdateEmployee(employeeId, updateDto));
    }
}