using SibersDataManager.Models.Projects;
using SibersDataManager.Models.Roles;
using SibersDataManager.Models.Tasks;

namespace SibersDataManager.Models.Employees;

public class EmployeeEntity
{
    
    public EmployeeEntity() {}
    
    public Guid Id { get; set; }
    
    public string Name { get; set; }
    public string SecondName { get; set; }
    public string ThirdName { get; set; }
    public string Email { get; set; }
    public Role Role { get; set; }
    
    public ICollection<ProjectEntity> Projects { get; set; } = new List<ProjectEntity>();
    
    public ICollection<ProjectEntity> ManagedProjects { get; set; } = new List<ProjectEntity>();
    
    public ICollection<ProjectTaskEntity> AuthoredTasks { get; set; } = new List<ProjectTaskEntity>();
    
    public ICollection<ProjectTaskEntity> WorkedTasks { get; set; } = new List<ProjectTaskEntity>();
    
    
}