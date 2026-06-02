using Microsoft.AspNetCore.Mvc;
using SibersDataManager.Models.Roles;
using SibersDataManager.Models.Tasks.Dto;
using SibersServices.Services.Task;

namespace SibersTestWork.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TaskController : ControllerBase
{
    private readonly ITaskService _service;
    private readonly ILogger<TaskController> _logger;

    public TaskController(ITaskService service, ILogger<TaskController> logger)
    {
        _service = service;
        _logger = logger;
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        _logger.LogInformation("Task GetById method called.");

        return Ok(await _service.GetById(id));
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAll(
        [FromQuery] TaskStatusEnum? status,
        [FromQuery] string? sortBy)
    {
        _logger.LogInformation("Task GetAll method called.");

        return Ok(
            await _service.GetFiltered(
                status,
                sortBy));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ProjectTaskToCreate dto)
    {
        _logger.LogInformation("Task GetById method called.");

        return Ok(await _service.CreateTask(dto));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] ProjectTaskToUpdate dto)
    {
        _logger.LogInformation("Task GetById method called.");

        return Ok(await _service.UpdateTask(id, dto));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        _logger.LogInformation("Task GetById method called.");

        return Ok(await _service.DeleteTaskById(id));
    }
}