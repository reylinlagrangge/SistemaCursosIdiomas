using Idiomas.Domain.Core;

namespace Idiomas.Domain.Entities;

public class Schedule : BaseEntity
{
    public DayOfWeek DayOfWeek { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public string? Classroom { get; set; }

    public int CourseId { get; set; }
    public Course Course { get; set; } = null!;
}