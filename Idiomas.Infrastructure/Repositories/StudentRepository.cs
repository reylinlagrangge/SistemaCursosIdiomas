using Idiomas.Domain.Entities;
using Idiomas.Infrastructure.Context;
using Idiomas.Infrastructure.Core;
using Idiomas.Infrastructure.Exceptions;
using Idiomas.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Idiomas.Infrastructure.Repositories;

public class StudentRepository : BaseRepository<Student>, IStudentRepository
{
    public StudentRepository(LanguageSchoolContext context) : base(context) { }

    public async Task<Student?> GetByEmailAsync(string email) =>
        await _set.AsNoTracking().FirstOrDefaultAsync(s => s.Email == email);

    public override async Task<Student> AddAsync(Student entity)
    {
        if (await _set.AnyAsync(s => s.Email == entity.Email))
            throw new StudentException("A student with that email already exists.");

        return await base.AddAsync(entity);
    }
}