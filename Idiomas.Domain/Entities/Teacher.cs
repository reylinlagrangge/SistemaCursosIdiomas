using Idiomas.Domain.Core;

namespace Idiomas.Domain.Entities;

public class Teacher : Person
{
    public string Specialty { get; set; } = string.Empty;

    public ICollection<Course> Courses { get; set; } = new List<Course>();
}