using Microsoft.AspNetCore.Mvc;
using SibersDataManager.Models.Tasks.Dto;
using SibersServices.Services.Task;

namespace SibersTestWork.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TaskController : ControllerBase
{
    private readonly ITaskService _service;

    public TaskController(ITaskService service)
    {
        _service = service;
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _service.GetById(id);
        return Ok(result);
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAll()
    {
        var result = await _service.GetAll();
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ProjectTaskToCreate dto)
    {
        var result = await _service.CreateTask(dto);
        return Ok(result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] ProjectTaskToUpdate dto)
    {
        var result = await _service.UpdateTask(id, dto);
        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _service.DeleteTaskById(id);
        return Ok(result);
    }
}