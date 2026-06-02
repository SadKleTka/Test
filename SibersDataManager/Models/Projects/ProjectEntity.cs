using SibersDataManager.Models.Employees;
using SibersDataManager.Models.Tasks;

namespace SibersDataManager.Models.Projects;

public class ProjectEntity
{
    
    public ProjectEntity() {}

    public ProjectEntity(string name, string customerCompany, string workerCompany, DateTime startDate, uint priority,
        DateTime? endDate, Guid? managerId)
    {
        Name = name;
        CustomerCompany = customerCompany;
        WorkerCompany = workerCompany;
        StartDate = startDate;
        Priority = priority;
        EndDate = endDate;
        ManagerId = managerId;
    }
    
    public Guid Id { get; set; }
    
    public string Name { get; set; }
    public string CustomerCompany { get; set; }
    public string WorkerCompany { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public uint Priority { get; set; }
    
    public Guid? ManagerId { get; set; }
    public EmployeeEntity? Manager { get; set; }
    
    public ICollection<EmployeeEntity> Employees { get; set; } = new List<EmployeeEntity>();
    
    public ICollection<ProjectTaskEntity> Tasks { get; set; } = new List<ProjectTaskEntity>();
    
    
}