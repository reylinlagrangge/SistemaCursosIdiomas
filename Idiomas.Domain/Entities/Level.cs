using Idiomas.Domain.Core;

namespace Idiomas.Domain.Entities;

public class Level : BaseEntity
{
    public string Name { get; set; } = string.Empty;   // e.g. A1, A2, B1
    public string? Description { get; set; }

    public ICollection<Course> Courses { get; set; } = new List<Course>();
}