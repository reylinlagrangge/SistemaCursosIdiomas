using Idiomas.Application.Dtos;
using Idiomas.Application.Dtos.Students;

namespace Idiomas.Application.Contract;

public interface IStudentService
{
    Task<ServiceResult<List<StudentDto>>> GetAllAsync();
    Task<ServiceResult<StudentDto>> GetByIdAsync(int id);
    Task<ServiceResult<StudentDto>> CreateAsync(SaveStudentDto dto);
    Task<ServiceResult<StudentDto>> UpdateAsync(int id, SaveStudentDto dto);
    Task<ServiceResult<bool>> DeleteAsync(int id);
}
