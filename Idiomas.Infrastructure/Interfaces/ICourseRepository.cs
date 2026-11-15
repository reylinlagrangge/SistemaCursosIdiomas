using Idiomas.Domain.Entities;
using Idiomas.Domain.Repository;

namespace Idiomas.Infrastructure.Interfaces;

public interface ICourseRepository : IBaseRepository<Course>
{
    Task<IEnumerable<Course>> GetByLevelAsync(int levelId);
}