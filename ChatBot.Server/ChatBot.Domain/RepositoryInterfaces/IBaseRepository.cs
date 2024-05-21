using ChatBot.Domain.Models.Base;
using System.Linq.Expressions;

namespace ChatBot.Domain.RepositoryInterfaces;

public interface IBaseRepository<TEntity, in TKey> : IRepository
    where TEntity : IBaseDomainModel<TKey>
{
    Task<TEntity?> GetByIdAsync(TKey id);

    Task<TEntity?> GetByIdAsync<TProperty>(TKey id, Expression<Func<TEntity, TProperty>> include);

    Task<TEntity?> GetByExpressionAsync(Expression<Func<TEntity, bool>> where);

    Task<TEntity?> GetByExpressionAsync<TProperty>(Expression<Func<TEntity, bool>> where, Expression<Func<TEntity, TProperty>> include);

    Task<IEnumerable<TEntity>> GetRangeByIdsAsync(IEnumerable<TKey> ids, bool asNoTracking = false);

    Task<IEnumerable<TEntity>> GetAllAsync(bool asNoTracking = false);

    Task<IEnumerable<TEntity>> GetAllAsync<TProperty>(Expression<Func<TEntity, TProperty>> include);

    Task<List<TEntity>> GetAllWithExpressionAsync(Expression<Func<TEntity, bool>> where, bool asNoTracking = false);

    Task<IEnumerable<TEntity>> GetAllWithExpressionAsync<TProperty>(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, TProperty>>[] includes);

    Task<IEnumerable<TEntity>> GetAllWithExpressionAndLimitAsync(Expression<Func<TEntity, bool>> where, int limit);

    Task DeleteAsync(TEntity entity);

    Task ModifyAsync(TEntity row);

    Task ModifyRangeAsync(IEnumerable<TEntity> rows);

    Task ModifyWithPropertiesWithoutModifyAsync<TProperty>(TEntity row, params Expression<Func<TEntity, TProperty>>[] propertiesWithoutModify);

    Task AddRangeAsync(IEnumerable<TEntity> rows);

    Task AddAsync(TEntity row);

    Task<bool> IsExistWithExpressionAsync(Expression<Func<TEntity, bool>> where);

    bool IsExistWithExpression(Expression<Func<TEntity, bool>> where);

    Task<bool> IsExistByIdAsync(TKey id);

    Task SaveChangesAsync();
    Task RemoveRangeAsync(List<TEntity> entities);
    void ChangeTrackerClear();
    Task ExecuteDeleteAsync(Expression<Func<TEntity, bool>> expression);
    Task RemoveAllAsync();
    Task ResetIdAsync();
}