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