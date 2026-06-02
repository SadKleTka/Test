using Microsoft.EntityFrameworkCore;
using SibersDataManager.Data;
using SibersDataManager.Models.Projects;

namespace SibersDataManager.Repository.ProjectRepository;

public class ProjectRepository : IProjectRepository
{
    private readonly AppDbContext _context;

    public ProjectRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<ProjectEntity>> GetFilteredAsync(
        uint? priority,
        string? sortBy)
    {
        var query = _context.Projects.AsQueryable();

        if (priority.HasValue)
        {
            query = query.Where(x => x.Priority == priority.Value);
        }

        query = sortBy?.ToLower() switch
        {
            "priority" => query.OrderBy(x => x.Priority),
            "startdate" => query.OrderBy(x => x.StartDate),
            "name" => query.OrderBy(x => x.Name),
            _ => query
        };

        return await query.ToListAsync();
    }

    public async Task PersistAsync(ProjectEntity entity)
    {
        await _context.Projects.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<ProjectEntity>> GetAllAsync()
    {
        return await _context.Projects.AsNoTracking().ToListAsync();
    }

    public async Task<ProjectEntity?> FindAsync(Guid id)
    {
        return await _context.Projects.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task UpdateAsync(ProjectEntity entity)
    {
        _context.Projects.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(ProjectEntity entity)
    {
        _context.Projects.Remove(entity);
        await _context.SaveChangesAsync();
    }
    
}