using Idiomas.Domain.Core;

namespace Idiomas.Domain.Entities;

public class Enrollment : BaseEntity
{
    public DateTime EnrollmentDate { get; set; } = DateTime.UtcNow;
    public string Status { get; set; } = "Active";   // Active, Withdrawn, Completed

    public int StudentId { get; set; }
    public Student Student { get; set; } = null!;

    public int CourseId { get; set; }
    public Course Course { get; set; } = null!;

    public ICollection<Payment> Payments { get; set; } = new List<Payment>();
    public ICollection<Grade> Grades { get; set; } = new List<Grade>();
}