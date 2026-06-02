using Microsoft.EntityFrameworkCore;
using SibersDataManager.Data;
using SibersDataManager.Models.Employees;
using SibersDataManager.Models.Projects;

namespace SibersDataManager.Repository.ProjectEmployee;

public class ProjectEmployeeRepository : IProjectEmployeeRepository
{
    private readonly AppDbContext _db;
    
    public ProjectEmployeeRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task LinkAsync(Guid projectId, Guid employeeId)
    {
        await _db.Database.ExecuteSqlRawAsync(
            "INSERT INTO \"EmployeeEntityProjectEntity\" (\"EmployeesId\", \"ProjectsId\") VALUES ({0}, {1})",
            employeeId, projectId
        );
    }


    public async Task UnLinkAsync(Guid projectId, Guid employeeId)
    {
        await _db.Database.ExecuteSqlRawAsync(
            "DELETE FROM \"EmployeeEntityProjectEntity\" WHERE \"EmployeesId\" = {0} AND \"ProjectsId\" = {1}",
            employeeId, projectId
        );
    }

    public async Task<ProjectEntity?> GetAllOfProject(Guid projectId)
    {
        return await _db.Projects.AsNoTracking().Include(e => e.Employees).FirstOrDefaultAsync(p => p.Id == projectId);
    }

    public async Task<EmployeeEntity?> GetEmployeeAsync(Guid employeeId)
    {
        return await _db.Employees.AsNoTracking().Include(e => e.Projects).FirstOrDefaultAsync(e => e.Id == employeeId);
    }
}