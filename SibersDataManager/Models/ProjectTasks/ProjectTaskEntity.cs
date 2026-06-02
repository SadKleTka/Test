using SibersDataManager.Models.Employees;
using SibersDataManager.Models.Projects;
using SibersDataManager.Models.Roles;

namespace SibersDataManager.Models.Tasks;

public class ProjectTaskEntity
{
    
    public ProjectTaskEntity() {} 

    public ProjectTaskEntity(string name, Guid authorId, Guid? executorId, TaskStatusEnum status, string comment, uint priority, Guid projectId)
    {
        Id = Guid.NewGuid();
        Name = name;
        AuthorId = authorId;
        WorkerId = executorId;
        Status = status;
        Comment = comment;
        Priority = priority;
        ProjectId = projectId;
    }    
    public Guid Id { get; set; }
    
    public string Name { get; set; }
    public string Comment { get; set; }
    public uint Priority { get; set; }
    public TaskStatusEnum Status { get; set; }
    
    public Guid ProjectId { get; set; }
    public ProjectEntity Project { get; set; }
    
    public Guid AuthorId { get; set; }
    public EmployeeEntity Author { get; set; }
    
    public Guid? WorkerId { get; set; }
    public EmployeeEntity? Worker { get; set; }
}