using Microsoft.EntityFrameworkCore;
using SibersDataManager.Data;
using SibersDataManager.Models.Tasks;

namespace SibersDataManager.Repository.ProjectTask;

public class ProjectTaskRepository : IProjectTaskRepository
{
    private readonly AppDbContext _context;

    public ProjectTaskRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ProjectTaskEntity?> FindAsync(Guid id) =>
        await _context.Tasks.FirstOrDefaultAsync(x => x.Id == id);

    public async Task<IEnumerable<ProjectTaskEntity>> GetAllAsync() =>
        await _context.Tasks.ToListAsync();

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