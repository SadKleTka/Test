using Microsoft.AspNetCore.Mvc;
using SibersDataManager.Models.Employees.Dto;
using SibersServices.Services.Employee;

namespace SibersTestWork.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeeController : ControllerBase
{
    private readonly ILogger<EmployeeController> _logger;
    private readonly IEmployeeService _service;

    public EmployeeController(IEmployeeService service, ILogger<EmployeeController> logger)
    {
        _logger = logger;
        _service = service;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        _logger.LogInformation("Employee GetById method called.");
        
        return Ok(await _service.GetById(id));
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        _logger.LogInformation("Employee GetAll method called.");

        return Ok(await _service.GetAll());
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] EmployeeToCreate dto)
    {
        _logger.LogInformation("Employee Create method called.");

        return Ok(await _service.CreateEmployee(dto));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] EmployeeToCreate dto)
    {
        _logger.LogInformation("Employee Update method called.");

        return Ok(await _service.UpdateEmployee(id, dto));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        _logger.LogInformation("Employee Delete method called.");

        return Ok(await _service.DeleteEmployeeById(id));
    }
}