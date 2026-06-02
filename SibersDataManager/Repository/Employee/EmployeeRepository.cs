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

    public async Task<EmployeeEntity> GetByEmailAsync(string email)
    {
        return await _context.Employees.FirstAsync(e => e.Email == email);
    }

    public async Task<EmployeeEntity?> FindAsync(Guid id) => 
        await _context.Employees.Include(
            p => p.ManagedProjects).
            Include(t => t.AuthoredTasks).
            Include(t => t.WorkedTasks).
            FirstAsync(x => x.Id == id);

    public async Task<IEnumerable<EmployeeEntity>> GetAllAsync() => 
        await _context.Employees.ToListAsync();

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