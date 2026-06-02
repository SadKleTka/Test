using SibersDataManager.Models.Employees;
using SibersDataManager.Models.Projects;

namespace SibersDataManager.Models.Tasks;

public class ProjectTaskEntity
{
    
    public ProjectTaskEntity() {}
    
    public Guid Id { get; set; }
    
    public string Name { get; set; }
    public string Comment { get; set; }
    public uint Priority { get; set; }
    public TaskStatus Status { get; set; }
    
    public Guid ProjectId { get; set; }
    public ProjectEntity Project { get; set; }
    
    public Guid AuthorId { get; set; }
    public EmployeeEntity Author { get; set; }
    
    public Guid? WorkerId { get; set; }
    public EmployeeEntity? Worker { get; set; }
}