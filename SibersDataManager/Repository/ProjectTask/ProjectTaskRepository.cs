using Microsoft.EntityFrameworkCore;
using SibersDataManager.Data;
using SibersDataManager.Models.Roles;
using SibersDataManager.Models.Tasks;

namespace SibersDataManager.Repository.ProjectTask;

public class ProjectTaskRepository : IProjectTaskRepository
{
    private readonly AppDbContext _context;

    public ProjectTaskRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<ProjectTaskEntity>> GetFilteredAsync(
        TaskStatusEnum? status,
        string? sortBy)
    {
        var query = _context.Tasks.AsQueryable();

        if (status.HasValue)
        {
            query = query.Where(x => x.Status == status.Value);
        }

        query = sortBy?.ToLower() switch
        {
            "priority" => query.OrderBy(x => x.Priority),
            "name" => query.OrderBy(x => x.Name),
            _ => query
        };

        return await query.ToListAsync();
    }

    public async Task<ProjectTaskEntity?> FindAsync(Guid id)
    {
        return await _context.Tasks.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<IEnumerable<ProjectTaskEntity>> GetAllAsync()
    {
        return await _context.Tasks.ToListAsync();
    }

    public async Task PersistAsync(ProjectTaskEntity entity)
    {
        await _context.Tasks.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(ProjectTaskEntity entity)
    {
        _context.Tasks.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(ProjectTaskEntity entity)
    {
        _context.Tasks.Remove(entity);
        await _context.SaveChangesAsync();
    }
}