using Idiomas.Domain.Entities;
using Idiomas.Domain.Repository;

namespace Idiomas.Infrastructure.Interfaces;

public interface IStudentRepository : IBaseRepository<Student>
{
    Task<Student?> GetByEmailAsync(string email);
}