namespace Idiomas.Infrastructure.Models;

public class CourseModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Language { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public int LevelId { get; set; }
    public int TeacherId { get; set; }
}