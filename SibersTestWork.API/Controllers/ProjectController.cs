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
    public async Task<ActionResult<IEnumerable<ProjectToResponse>>> GetAll(
        [FromQuery] uint? priority,
        [FromQuery] string? sortBy)
    {
        _logger.LogInformation("Project GetAll method called.");

        return Ok(
            await _service.GetFiltered(
                priority,
                sortBy));
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

    [HttpPost("with-id")]
    public async Task<ActionResult<ProjectToResponse>> CreateProjectWithId
    (
        [FromBody]
        ProjectToCreate project
    )
    {
        _logger.LogInformation("Project CreateProjectWithId method called.");

        return Ok(await _service.CreateProjectAndReturn(project));
    }

    [HttpPost("{id:guid}/documents")]
    [RequestSizeLimit(50_000_000)]
    public async Task<ActionResult> UploadDocuments
    (
        [FromRoute]
        Guid id,
        [FromForm]
        List<IFormFile> files
    )
    {
        _logger.LogInformation("Project UploadDocuments method called for project {ProjectId}.", id);

        if (files is null || files.Count == 0)
            return BadRequest(new Message("No documents were uploaded", DateTime.UtcNow));

        var uploadDirectory = Path.Combine(
            Directory.GetCurrentDirectory(),
            "wwwroot",
            "uploads",
            "projects",
            id.ToString());

        Directory.CreateDirectory(uploadDirectory);

        var savedFiles = new List<object>();

        foreach (var file in files.Where(file => file.Length > 0))
        {
            var originalFileName = Path.GetFileName(file.FileName);
            var storedFileName = $"{Guid.NewGuid():N}_{originalFileName}";
            var filePath = Path.Combine(uploadDirectory, storedFileName);

            await using var stream = System.IO.File.Create(filePath);
            await file.CopyToAsync(stream);

            savedFiles.Add(new
            {
                fileName = originalFileName,
                url = $"/uploads/projects/{id}/{Uri.EscapeDataString(storedFileName)}",
                size = file.Length
            });
        }

        return Ok(new
        {
            message = "Documents uploaded successfully",
            files = savedFiles
        });
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