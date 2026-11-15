using Idiomas.Domain.Core;

namespace Idiomas.Domain.Entities;

public class Grade : BaseEntity
{
    public string Evaluation { get; set; } = string.Empty;   // e.g. Midterm exam
    public decimal Score { get; set; }
    public DateTime RecordedAt { get; set; } = DateTime.UtcNow;

    public int EnrollmentId { get; set; }
    public Enrollment Enrollment { get; set; } = null!;
}