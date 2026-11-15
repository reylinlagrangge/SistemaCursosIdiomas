using Idiomas.Application.Contract;
using Idiomas.Application.Dtos;
using Idiomas.Application.Dtos.Courses;
using Idiomas.Domain.Entities;
using Idiomas.Infrastructure.Exceptions;
using Idiomas.Infrastructure.Interfaces;

namespace Idiomas.Application.Services;

public class CourseService : ICourseService
{
    private readonly ICourseRepository _repo;

    public CourseService(ICourseRepository repo) => _repo = repo;

    private static string? Validate(SaveCourseDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name)) return "El nombre es requerido.";
        if (dto.Name.Length > 100) return "El nombre no puede pasar de 100 caracteres.";
        if (string.IsNullOrWhiteSpace(dto.Language)) return "El idioma es requerido.";
        if (dto.Language.Length > 50) return "El idioma no puede pasar de 50 caracteres.";
        if (dto.Description is { Length: > 500 }) return "La descripción no puede pasar de 500 caracteres.";
        if (dto.Price <= 0) return "El precio debe ser mayor que 0.";
        if (dto.LevelId <= 0) return "El nivel es requerido.";
        if (dto.TeacherId <= 0) return "El profesor es requerido.";
        return null;
    }

    private static CourseDto Map(Course c) => new()
    {
        Id = c.Id,
        Name = c.Name,
        Language = c.Language,
        Description = c.Description,
        Price = c.Price,
        LevelId = c.LevelId,
        TeacherId = c.TeacherId
    };

    public async Task<ServiceResult<List<CourseDto>>> GetAllAsync()
    {
        var courses = await _repo.GetAllAsync();
        return ServiceResult<List<CourseDto>>.Ok(courses.Select(Map).ToList());
    }

    public async Task<ServiceResult<CourseDto>> GetByIdAsync(int id)
    {
        var course = await _repo.GetByIdAsync(id);
        return course is null
            ? ServiceResult<CourseDto>.NotFoundResult("Curso no encontrado.")
            : ServiceResult<CourseDto>.Ok(Map(course));
    }

    public async Task<ServiceResult<List<CourseDto>>> GetByLevelAsync(int levelId)
    {
        if (levelId <= 0) return ServiceResult<List<CourseDto>>.Fail("El nivel es inválido.");
        var courses = await _repo.GetByLevelAsync(levelId);
        return ServiceResult<List<CourseDto>>.Ok(courses.Select(Map).ToList());
    }

    public async Task<ServiceResult<CourseDto>> CreateAsync(SaveCourseDto dto)
    {
        var error = Validate(dto);
        if (error != null) return ServiceResult<CourseDto>.Fail(error);

        try
        {
            var created = await _repo.AddAsync(new Course
            {
                Name = dto.Name.Trim(),
                Language = dto.Language.Trim(),
                Description = dto.Description,
                Price = dto.Price,
                LevelId = dto.LevelId,
                TeacherId = dto.TeacherId
            });
            return ServiceResult<CourseDto>.Ok(Map(created));
        }
        catch (CourseException ex)
        {
            return ServiceResult<CourseDto>.Fail(ex.Message);
        }
    }

    public async Task<ServiceResult<CourseDto>> UpdateAsync(int id, SaveCourseDto dto)
    {
        var error = Validate(dto);
        if (error != null) return ServiceResult<CourseDto>.Fail(error);

        var course = await _repo.GetByIdAsync(id);
        if (course is null) return ServiceResult<CourseDto>.NotFoundResult("Curso no encontrado.");

        course.Name = dto.Name.Trim();
        course.Language = dto.Language.Trim();
        course.Description = dto.Description;
        course.Price = dto.Price;
        course.LevelId = dto.LevelId;
        course.TeacherId = dto.TeacherId;

        try
        {
            await _repo.UpdateAsync(course);
            return ServiceResult<CourseDto>.Ok(Map(course));
        }
        catch (CourseException ex)
        {
            return ServiceResult<CourseDto>.Fail(ex.Message);
        }
    }

    public async Task<ServiceResult<bool>> DeleteAsync(int id)
    {
        if (await _repo.GetByIdAsync(id) is null)
            return ServiceResult<bool>.NotFoundResult("Curso no encontrado.");

        await _repo.DeleteAsync(id);
        return ServiceResult<bool>.Ok(true);
    }
}
