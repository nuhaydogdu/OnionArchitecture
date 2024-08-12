using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using OnionArchitecture.Application.Abstractions.Repositories;
using OnionArchitecture.Domain.Shared;
using OnionArchitecture.Persistence.Contexts;

namespace OnionArchitecture.Persistence.Repositories;

public class ReadRepository<T> : IReadRepository<T> where T : Entity
{
    private readonly OnionArchitectureDbContext _dbContext;

    public ReadRepository(OnionArchitectureDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public DbSet<T> Table => _dbContext.Set<T>();
    
    public IQueryable<T>? GetAll(bool tracking = true)
    {
        var query = Table.Where(x => !x.IsDeleted).AsQueryable();
        if (!tracking)
            query = query.AsNoTracking();
        return query;
    }

    public IQueryable<T?> GetWhere(Expression<Func<T, bool>> method, bool tracking = true)
    {
        var query = Table.Where(method);
        if (!tracking)
            query = query.AsNoTracking();
        return query;
    }

    public async Task<T?> GetSingleAsync(Expression<Func<T, bool>> method, bool tracking = true)
    {
        var query = Table.AsQueryable();
        if (!tracking)
            query = Table.AsNoTracking();
        return await query.FirstOrDefaultAsync(method);
    }

    public async Task<T?> GetByIdAsync(Guid id, bool tracking = true)
    {
        var query = Table.AsQueryable();
        if (!tracking)
            query = Table.AsNoTracking();
        return await query.FirstOrDefaultAsync(data => data.Id == id);
    }
}