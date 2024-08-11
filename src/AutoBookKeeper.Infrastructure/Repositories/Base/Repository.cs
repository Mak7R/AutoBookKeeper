using System.Linq.Expressions;
using AutoBookKeeper.Core.Entities.Base;
using AutoBookKeeper.Core.Models;
using AutoBookKeeper.Core.Repositories.Base;
using AutoBookKeeper.Core.Specifications.Base;
using AutoBookKeeper.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AutoBookKeeper.Infrastructure.Repositories.Base;

public class Repository<TEntity, TId> : IRepository<TEntity, TId>
    where TEntity: Entity<TId>
{
    protected readonly ApplicationDbContext DbContext;

    public Repository(ApplicationDbContext dbContext)
    {
        DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }
    
    private IQueryable<TEntity> ApplySpecification(ISpecification<TEntity> spec)
    {
        return SpecificationEvaluator<TEntity, TId>.GetQuery(DbContext.Set<TEntity>().AsQueryable(), spec);
    }
    
    public async Task<IReadOnlyList<TEntity>> GetAllAsync()
    {
        return await DbContext.Set<TEntity>().ToListAsync();
    }

    public async Task<IReadOnlyList<TEntity>> GetAsync(ISpecification<TEntity> spec)
    {
        return await ApplySpecification(spec).ToListAsync();
    }

    public Task<TEntity> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<OperationResult<TEntity>> CreateAsync(TEntity entity)
    {
        throw new NotImplementedException();
    }

    public Task<OperationResult<TEntity>> UpdateAsync(TEntity entity)
    {
        throw new NotImplementedException();
    }

    public Task<OperationResult<TEntity>> DeleteAsync(TEntity entity)
    {
        throw new NotImplementedException();
    }

    public Task<int> CountAsync(ISpecification<TEntity> spec)
    {
        throw new NotImplementedException();
    }
}