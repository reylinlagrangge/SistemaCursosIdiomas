using System.Net.Mail;
using Idiomas.Application.Contract;
using Idiomas.Application.Dtos;
using Idiomas.Application.Dtos.Students;
using Idiomas.Domain.Entities;
using Idiomas.Infrastructure.Exceptions;
using Idiomas.Infrastructure.Interfaces;

namespace Idiomas.Application.Services;

public class StudentService : IStudentService
{
    private readonly IStudentRepository _repo;

    public StudentService(IStudentRepository repo) => _repo = repo;

    private static string? Validate(SaveStudentDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.FirstName)) return "El nombre es requerido.";
        if (dto.FirstName.Length > 100) return "El nombre no puede pasar de 100 caracteres.";
        if (string.IsNullOrWhiteSpace(dto.LastName)) return "El apellido es requerido.";
        if (dto.LastName.Length > 100) return "El apellido no puede pasar de 100 caracteres.";
        if (string.IsNullOrWhiteSpace(dto.Email)) return "El correo es requerido.";
        if (dto.Email.Length > 150) return "El correo no puede pasar de 150 caracteres.";
        if (!MailAddress.TryCreate(dto.Email, out _)) return "El correo no tiene un formato válido.";
        if (dto.Phone is { Length: > 20 }) return "El teléfono no puede pasar de 20 caracteres.";
        if (dto.BirthDate.HasValue && dto.BirthDate.Value.Date > DateTime.Today)
            return "La fecha de nacimiento no puede ser futura.";
        if (string.IsNullOrWhiteSpace(dto.StudentNumber)) return "El número de estudiante es requerido.";
        if (dto.StudentNumber.Length > 20) return "El número de estudiante no puede pasar de 20 caracteres.";
        return null;
    }

    private static StudentDto Map(Student s) => new()
    {
        Id = s.Id,
        FirstName = s.FirstName,
        LastName = s.LastName,
        Email = s.Email,
        Phone = s.Phone,
        BirthDate = s.BirthDate,
        StudentNumber = s.StudentNumber
    };

    public async Task<ServiceResult<List<StudentDto>>> GetAllAsync()
    {
        var students = await _repo.GetAllAsync();
        return ServiceResult<List<StudentDto>>.Ok(students.Select(Map).ToList());
    }

    public async Task<ServiceResult<StudentDto>> GetByIdAsync(int id)
    {
        var student = await _repo.GetByIdAsync(id);
        return student is null
            ? ServiceResult<StudentDto>.NotFoundResult("Estudiante no encontrado.")
            : ServiceResult<StudentDto>.Ok(Map(student));
    }

    public async Task<ServiceResult<StudentDto>> CreateAsync(SaveStudentDto dto)
    {
        var error = Validate(dto);
        if (error != null) return ServiceResult<StudentDto>.Fail(error);

        if (await _repo.GetByEmailAsync(dto.Email.Trim()) != null)
            return ServiceResult<StudentDto>.Fail("Ya existe un estudiante con ese correo.");

        try
        {
            var created = await _repo.AddAsync(new Student
            {
                FirstName = dto.FirstName.Trim(),
                LastName = dto.LastName.Trim(),
                Email = dto.Email.Trim(),
                Phone = dto.Phone,
                BirthDate = dto.BirthDate,
                StudentNumber = dto.StudentNumber.Trim()
            });
            return ServiceResult<StudentDto>.Ok(Map(created));
        }
        catch (StudentException ex)
        {
            return ServiceResult<StudentDto>.Fail(ex.Message);
        }
    }

    public async Task<ServiceResult<StudentDto>> UpdateAsync(int id, SaveStudentDto dto)
    {
        var error = Validate(dto);
        if (error != null) return ServiceResult<StudentDto>.Fail(error);

        var student = await _repo.GetByIdAsync(id);
        if (student is null) return ServiceResult<StudentDto>.NotFoundResult("Estudiante no encontrado.");

        var sameEmail = await _repo.GetByEmailAsync(dto.Email.Trim());
        if (sameEmail != null && sameEmail.Id != id)
            return ServiceResult<StudentDto>.Fail("Ya existe otro estudiante con ese correo.");

        student.FirstName = dto.FirstName.Trim();
        student.LastName = dto.LastName.Trim();
        student.Email = dto.Email.Trim();
        student.Phone = dto.Phone;
        student.BirthDate = dto.BirthDate;
        student.StudentNumber = dto.StudentNumber.Trim();

        try
        {
            await _repo.UpdateAsync(student);
            return ServiceResult<StudentDto>.Ok(Map(student));
        }
        catch (StudentException ex)
        {
            return ServiceResult<StudentDto>.Fail(ex.Message);
        }
    }

    public async Task<ServiceResult<bool>> DeleteAsync(int id)
    {
        if (await _repo.GetByIdAsync(id) is null)
            return ServiceResult<bool>.NotFoundResult("Estudiante no encontrado.");

        await _repo.DeleteAsync(id);
        return ServiceResult<bool>.Ok(true);
    }
}
