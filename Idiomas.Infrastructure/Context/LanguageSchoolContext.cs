using Idiomas.Domain.Core;
using Idiomas.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Idiomas.Infrastructure.Context;

public class LanguageSchoolContext : DbContext
{
    public LanguageSchoolContext(DbContextOptions<LanguageSchoolContext> options) : base(options) { }

    public DbSet<Student> Students => Set<Student>();
    public DbSet<Teacher> Teachers => Set<Teacher>();
    public DbSet<Level> Levels => Set<Level>();
    public DbSet<Course> Courses => Set<Course>();
    public DbSet<Schedule> Schedules => Set<Schedule>();
    public DbSet<Enrollment> Enrollments => Set<Enrollment>();
    public DbSet<Payment> Payments => Set<Payment>();
    public DbSet<Grade> Grades => Set<Grade>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Ignore<BaseEntity>();
        modelBuilder.Ignore<Person>();

        modelBuilder.Entity<Course>().Property(c => c.Price).HasPrecision(18, 2);
        modelBuilder.Entity<Payment>().Property(p => p.Amount).HasPrecision(18, 2);
        modelBuilder.Entity<Grade>().Property(g => g.Score).HasPrecision(5, 2);

        modelBuilder.Entity<Course>().Property(c => c.Name).HasMaxLength(100).IsRequired();
        modelBuilder.Entity<Student>().Property(s => s.FirstName).HasMaxLength(100).IsRequired();
        modelBuilder.Entity<Student>().Property(s => s.Email).HasMaxLength(150).IsRequired();

        // Avoids "multiple cascade paths" errors in SQL Server
        foreach (var fk in modelBuilder.Model.GetEntityTypes().SelectMany(t => t.GetForeignKeys()))
            fk.DeleteBehavior = DeleteBehavior.Restrict;

        base.OnModelCreating(modelBuilder);
    }
}