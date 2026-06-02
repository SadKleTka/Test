using Microsoft.EntityFrameworkCore;
using SibersDataManager.Data;
using SibersDataManager.Models.Employees;

namespace SibersDataManager.Repository.Employee;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly AppDbContext _context;

    public EmployeeRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<EmployeeEntity?> GetByEmailAsync(string email)
    {
        return await _context.Employees.FirstOrDefaultAsync(e => e.Email == email);
    }

    public async Task<EmployeeEntity?> FindAsync(Guid id) =>
        await _context.Employees
            .Include(p => p.ManagedProjects)
            .Include(t => t.AuthoredTasks)
            .Include(t => t.WorkedTasks)
            .FirstOrDefaultAsync(x => x.Id == id);

    public async Task<IEnumerable<EmployeeEntity>> GetAllAsync(string? search = null)
    {
        var query = _context.Employees.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var normalizedSearch = search.Trim().ToLower();
            query = query.Where(employee =>
                (employee.Name ?? string.Empty).ToLower().Contains(normalizedSearch) ||
                (employee.SecondName ?? string.Empty).ToLower().Contains(normalizedSearch) ||
                (employee.ThirdName ?? string.Empty).ToLower().Contains(normalizedSearch) ||
                (employee.Email ?? string.Empty).ToLower().Contains(normalizedSearch));
        }

        return await query
            .OrderBy(employee => employee.SecondName)
            .ThenBy(employee => employee.Name)
            .Take(25)
            .ToListAsync();
    }

    public async Task PersistAsync(EmployeeEntity entity)
    {
        await _context.Employees.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(EmployeeEntity entity)
    {
        _context.Employees.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(EmployeeEntity entity)
    {
        _context.Employees.Remove(entity);
        await _context.SaveChangesAsync();
    }
}
