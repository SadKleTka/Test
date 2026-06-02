using Microsoft.EntityFrameworkCore;
using SibersDataManager.Models.Employees;
using SibersDataManager.Models.Projects;
using SibersDataManager.Models.Tasks;

namespace SibersDataManager.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) {}
    
    public DbSet<ProjectEntity> Projects => Set<ProjectEntity>();
    
    public DbSet<EmployeeEntity> Employees => Set<EmployeeEntity>();
    
    public DbSet<ProjectTaskEntity> Tasks => Set<ProjectTaskEntity>();
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EmployeeEntity>()
            .HasMany(e => e.Projects)
            .WithMany(p => p.Employees);
        
        modelBuilder.Entity<EmployeeEntity>()
            .HasMany(e => e.ManagedProjects)
            .WithOne(p => p.Manager)
            .HasForeignKey(p => p.ManagerId)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<EmployeeEntity>()
            .HasMany(e => e.AuthoredTasks)
            .WithOne(p => p.Author)
            .HasForeignKey(p => p.AuthorId)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<EmployeeEntity>()
            .HasMany(e => e.WorkedTasks)
            .WithOne(p => p.Worker)
            .HasForeignKey(w => w.WorkerId)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<ProjectEntity>()
            .HasMany(p => p.Tasks)
            .WithOne(p => p.Project)
            .HasForeignKey(p => p.ProjectId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}