using Microsoft.EntityFrameworkCore;
using TravelReclaim.Domain.Interfaces;

namespace TravelReclaim.Infrastructure.Persistence.SqlServer.Repositories;

public class RepositoryBase<T> : IRepository<T> where T : class
{
    protected readonly AppDbContext _context;
    protected readonly DbSet<T> Dbset;

    public RepositoryBase(AppDbContext context)
    {
        _context = context;
        Dbset = context.Set<T>();
    }

    public async Task<T> AddAsync(T entity, CancellationToken ct = default)
    {
        await Dbset.AddAsync(entity, ct);
        await _context.SaveChangesAsync(ct);
        return entity;
    }

    public async Task DeleteAsync(T entity, CancellationToken ct = default)
    {
        Dbset.Remove(entity);
        await _context.SaveChangesAsync(ct);
    }

    public async Task<IReadOnlyList<T>> GetAllAsync(CancellationToken ct = default)
    {
        return await Dbset.AsNoTracking().ToListAsync(ct);
    }

    public async Task<T?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await Dbset.FindAsync([id], ct);
    }

    public async Task UpdateAsync(T entity, CancellationToken ct = default)
    {
        _context.Entry(entity).State = EntityState.Modified;
        await _context.SaveChangesAsync(ct);
    }
}