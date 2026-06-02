using Microsoft.AspNetCore.Mvc;
using SibersDataManager.Models;
using SibersServices.Services.ProjectEmployee;

namespace SibersTestWork.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProjectEmployeeController : ControllerBase
{
    private readonly ILogger<ProjectEmployeeController> _logger;
    private readonly IProjectEmployeeService _service;
    
    public ProjectEmployeeController(ILogger<ProjectEmployeeController> logger, IProjectEmployeeService service)
    {
        _logger = logger;
        _service = service;
    }

    [HttpPost]
    public async Task<ActionResult<Message>> LinkEmployeeAndProject
    (
        Guid projectId,
        Guid employeeId
    )
    {
        _logger.LogInformation("LinkEmployeeAndProject method called.");

        return Ok(await _service.LinkEmployeeAndProject(projectId, employeeId));
    }

    [HttpPost("unlink")]
    public async Task<ActionResult<Message>> UnlinkEmployeeAndProject(Guid projectId, Guid employeeId)
    {
        _logger.LogInformation("UnLinkProjectEmployee method called.");

        return Ok(await _service.UnlinkEmployeeAndProject(projectId, employeeId));
    }
}