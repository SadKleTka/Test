using Microsoft.AspNetCore.Mvc;
using SibersDataManager.Models;
using SibersDataManager.Models.Projects.Dto;
using SibersServices.Services;

namespace SibersTestWork.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProjectController : ControllerBase
{
    private readonly IProjectService _service;
    private readonly ILogger<ProjectController> _logger;
    
    public ProjectController(IProjectService service, ILogger<ProjectController> logger)
    {
        _service = service;
        _logger = logger;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProjectToResponse>> GetById([FromRoute] Guid id)
    {
        _logger.LogInformation("Project GetById method called.");

        return Ok(await _service.GetById(id));
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProjectToResponse>>> GetAll()
    {
        _logger.LogInformation("Project GetAll Projects method called.");

        return Ok(await _service.GetAll());
    }

    [HttpPost]
    public async Task<ActionResult<Message>> CreateProject
    (
        [FromBody]
        ProjectToCreate project
    )
    {
        _logger.LogInformation("Project CreateProject method called.");

        return Ok(await _service.CreateProject(project));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Message>> UpdateProject
    (
        [FromRoute]
        Guid id,
        [FromBody]
        ProjectToUpdate project
    )
    {
        _logger.LogInformation("Project UpdateProject Method called.");
        
        return Ok(await _service.UpdateProject(id, project));
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<Message>> DeleteProjectById
    (
        [FromRoute]
        Guid id
    )
    {
        _logger.LogInformation("Project DeleteProject Method called.");

        return Ok(await _service.DeleteProjectById(id));
    }
}