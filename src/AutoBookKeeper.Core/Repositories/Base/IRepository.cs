using System.Linq.Expressions;
using AutoBookKeeper.Core.Entities.Base;
using AutoBookKeeper.Core.Models;
using AutoBookKeeper.Core.Specifications.Base;

namespace AutoBookKeeper.Core.Repositories.Base;

public interface IRepository<TEntity, TId> where TEntity: Entity<TId>
{
    Task<IReadOnlyList<TEntity>> GetAllAsync();
    Task<IReadOnlyList<TEntity>> GetAsync(ISpecification<TEntity> spec);
    Task<int> CountAsync(ISpecification<TEntity> spec);
    Task<TEntity> GetByIdAsync(int id);
    
    Task<OperationResult<TEntity>> CreateAsync(TEntity entity);
    Task<OperationResult<TEntity>> UpdateAsync(TEntity entity);
    Task<OperationResult<TEntity>> DeleteAsync(TEntity entity);
}