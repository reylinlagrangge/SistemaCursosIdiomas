using Idiomas.Domain.Core;
using Idiomas.Domain.Repository;
using Idiomas.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Idiomas.Infrastructure.Core;

public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
{
    protected readonly LanguageSchoolContext _context;
    protected readonly DbSet<T> _set;

    public BaseRepository(LanguageSchoolContext context)
    {
        _context = context;
        _set = context.Set<T>();
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync() =>
        await _set.AsNoTracking().ToListAsync();

    public virtual async Task<T?> GetByIdAsync(int id) =>
        await _set.FindAsync(id);

    public virtual async Task<T> AddAsync(T entity)
    {
        _set.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public virtual async Task UpdateAsync(T entity)
    {
        _set.Update(entity);
        await _context.SaveChangesAsync();
    }

    public virtual async Task DeleteAsync(int id)
    {
        var entity = await _set.FindAsync(id);
        if (entity is null) return;
        _set.Remove(entity);
        await _context.SaveChangesAsync();
    }
}