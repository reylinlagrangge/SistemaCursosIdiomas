using Idiomas.Domain.Core;

namespace Idiomas.Domain.Entities;

public class Payment : BaseEntity
{
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; } = DateTime.UtcNow;
    public string Method { get; set; } = string.Empty;   // Cash, Card, Transfer
    public string? Description { get; set; }

    public int EnrollmentId { get; set; }
    public Enrollment Enrollment { get; set; } = null!;
}