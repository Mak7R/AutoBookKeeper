using AutoBookKeeper.Core.Entities.Base;
using AutoBookKeeper.Core.Models;
using AutoBookKeeper.Core.Repositories.Base;
using AutoBookKeeper.Core.Specifications.Base;
using AutoBookKeeper.Infrastructure.Data;
using AutoBookKeeper.Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AutoBookKeeper.Infrastructure.Repositories.Base;

public class Repository<TEntity, TId> : IRepository<TEntity, TId>
    where TEntity: Entity<TId>
{
    protected readonly ApplicationDbContext DbContext;
    private readonly ILogger<Repository<TEntity, TId>> _logger;

    public Repository(ApplicationDbContext dbContext, ILogger<Repository<TEntity, TId>> logger)
    {
        DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _logger = logger;
    }
    
    private IQueryable<TEntity> ApplySpecification(ISpecification<TEntity> spec)
    {
        return SpecificationEvaluator<TEntity, TId>.GetQuery(DbContext.Set<TEntity>().AsNoTracking().AsQueryable(), spec);
    }
    
    public async Task<IReadOnlyList<TEntity>> GetAllAsync()
    {
        return await DbContext.Set<TEntity>().ToListAsync();
    }

    public async Task<IReadOnlyList<TEntity>> GetAsync(ISpecification<TEntity> spec)
    {
        return await ApplySpecification(spec).ToListAsync();
    }

    public async Task<int> CountAsync()
    {
        return await DbContext.Set<TEntity>().CountAsync();
    }
    
    public async Task<int> CountAsync(ISpecification<TEntity> spec)
    {
        return await ApplySpecification(spec).CountAsync();
    }
    
    public async Task<TEntity?> GetByIdAsync(TId id)
    {
        return await DbContext.Set<TEntity>().AsNoTracking().SingleOrDefaultAsync();
    }

    public async Task<OperationResult<TEntity>> CreateAsync(TEntity entity)
    {
        try
        {
            await DbContext.Set<TEntity>().AddAsync(entity);
            await DbContext.SaveChangesAsync();
            return OperationResult<TEntity>.FromResult(entity);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error was occured while creating entity {entity}", entity);
            return OperationResult<TEntity>.FromException(new DataBaseException("Error was occured while creating entity", e));
        }
    }

    public async Task<OperationResult<TEntity>> UpdateAsync(TEntity entity)
    {
        try
        {
            DbContext.Entry(entity).State = EntityState.Modified;
            await DbContext.SaveChangesAsync();
            return OperationResult<TEntity>.FromResult(entity);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error was occured while updating entity {entity}", entity);
            return OperationResult<TEntity>.FromException(new DataBaseException("Error was occured while updating entity", e));
        }
    }

    public async Task<OperationResult<TEntity>> DeleteAsync(TEntity entity)
    {
        try
        {
            DbContext.Set<TEntity>().Remove(entity);
            await DbContext.SaveChangesAsync();
            return OperationResult<TEntity>.FromResult(entity);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error was occured while deleting entity {entity}", entity);
            return OperationResult<TEntity>.FromException(new DataBaseException("Error was occured while deleting entity", e));
        }
    }
}