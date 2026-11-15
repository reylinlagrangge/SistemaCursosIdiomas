using Idiomas.Domain.Core;

namespace Idiomas.Domain.Entities;

public class Course : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Language { get; set; } = string.Empty;   // e.g. English, French
    public string? Description { get; set; }
    public decimal Price { get; set; }

    public int LevelId { get; set; }
    public Level Level { get; set; } = null!;

    public int TeacherId { get; set; }
    public Teacher Teacher { get; set; } = null!;

    public ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
    public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
}