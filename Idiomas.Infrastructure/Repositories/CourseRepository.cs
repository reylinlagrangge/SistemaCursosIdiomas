using Idiomas.Domain.Entities;
using Idiomas.Infrastructure.Context;
using Idiomas.Infrastructure.Core;
using Idiomas.Infrastructure.Exceptions;
using Idiomas.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Idiomas.Infrastructure.Repositories;

public class CourseRepository : BaseRepository<Course>, ICourseRepository
{
    public CourseRepository(LanguageSchoolContext context) : base(context) { }

    public async Task<IEnumerable<Course>> GetByLevelAsync(int levelId) =>
        await _set.AsNoTracking().Where(c => c.LevelId == levelId).ToListAsync();

    public override async Task<Course> AddAsync(Course entity)
    {
        if (await _set.AnyAsync(c => c.Name == entity.Name && c.LevelId == entity.LevelId))
            throw new CourseException("A course with that name already exists in this level.");

        return await base.AddAsync(entity);
    }
}