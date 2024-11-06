using AutoBookKeeper.Core.Entities.Base;
using AutoBookKeeper.Core.Repositories.Base;
using AutoBookKeeper.Core.Specifications.Base;
using AutoBookKeeper.Infrastructure.Data;
using AutoBookKeeper.Infrastructure.Helpers;
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
    
    public virtual async Task<IReadOnlyList<TEntity>> GetAllAsync()
    {
        try
        {
            return await DbContext.Set<TEntity>().ToListAsync();
        }
        catch (Exception e)
        {
            throw InfrastructureExceptionsHandlingHelper.Handle(e, _logger);
        }
    }

    public virtual async Task<IReadOnlyList<TEntity>> GetAsync(ISpecification<TEntity> spec)
    {
        try
        {
            return await ApplySpecification(spec).ToListAsync();
        }
        catch (Exception e)
        {
            throw InfrastructureExceptionsHandlingHelper.Handle(e, _logger);
        }
    }

    public virtual async Task<int> CountAsync()
    {
        try
        {
            return await DbContext.Set<TEntity>().CountAsync();
        }
        catch (Exception e)
        {
            throw InfrastructureExceptionsHandlingHelper.Handle(e, _logger);
        }
    }
    
    public virtual async Task<int> CountAsync(ISpecification<TEntity> spec)
    {
        try
        {
            return await ApplySpecification(spec).CountAsync();
        }
        catch (Exception e)
        {
            throw InfrastructureExceptionsHandlingHelper.Handle(e, _logger);
        }
    }
    
    public virtual async Task<TEntity?> GetByIdAsync(TId id)
    {
        try
        {
            return await DbContext.Set<TEntity>()
                .AsNoTracking()
                .Where(e => e.Id != null && e.Id.Equals(id))
                .SingleOrDefaultAsync();
        }
        catch (Exception e)
        {
            throw InfrastructureExceptionsHandlingHelper.Handle(e, _logger);
        }
    }

    public virtual async Task<TEntity> CreateAsync(TEntity entity)
    {
        try
        {
            await DbContext.Set<TEntity>().AddAsync(entity);
            await DbContext.SaveChangesAsync();
            return entity;
        }
        catch (DbUpdateException dbUpdateException)
        {
            throw InfrastructureExceptionsHandlingHelper.Handle(dbUpdateException, _logger, "CREATE");
        }
        catch (Exception e)
        {
            throw InfrastructureExceptionsHandlingHelper.Handle(e, _logger);
        }
    }

    public virtual async Task<TEntity> UpdateAsync(TEntity entity)
    {
        try
        {
            DbContext.Entry(entity).State = EntityState.Modified;
            await DbContext.SaveChangesAsync();
            return entity;
        }
        catch (DbUpdateException dbUpdateException)
        {
            throw InfrastructureExceptionsHandlingHelper.Handle(dbUpdateException, _logger, "UPDATE");
        }
        catch (Exception e)
        {
            throw InfrastructureExceptionsHandlingHelper.Handle(e, _logger);
        }
    }

    public virtual async Task<TEntity> DeleteAsync(TEntity entity)
    {
        try
        {
            DbContext.Set<TEntity>().Remove(entity);
            await DbContext.SaveChangesAsync();
            return entity;
        }
        catch (DbUpdateException dbUpdateException)
        {
            throw InfrastructureExceptionsHandlingHelper.Handle(dbUpdateException, _logger, "DELETE");
        }
        catch (Exception e)
        {
            throw InfrastructureExceptionsHandlingHelper.Handle(e, _logger);
        }
    }
}