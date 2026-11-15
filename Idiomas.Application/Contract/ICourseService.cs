using Idiomas.Application.Dtos;
using Idiomas.Application.Dtos.Courses;

namespace Idiomas.Application.Contract;

public interface ICourseService
{
    Task<ServiceResult<List<CourseDto>>> GetAllAsync();
    Task<ServiceResult<CourseDto>> GetByIdAsync(int id);
    Task<ServiceResult<List<CourseDto>>> GetByLevelAsync(int levelId);
    Task<ServiceResult<CourseDto>> CreateAsync(SaveCourseDto dto);
    Task<ServiceResult<CourseDto>> UpdateAsync(int id, SaveCourseDto dto);
    Task<ServiceResult<bool>> DeleteAsync(int id);
}
