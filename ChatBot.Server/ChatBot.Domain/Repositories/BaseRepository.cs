using ChatBot.Domain.Models.Base;
using ChatBot.Domain.RepositoryInterfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ChatBot.Domain.Repositories;

public abstract class BaseRepository<TEntity, TKey> : IBaseRepository<TEntity, TKey>
        where TEntity : class, IBaseDomainModel<TKey>
{
    protected BaseRepository(ApplicationContext context)
    {
        Context = context ?? throw new NullReferenceException();
        TableOriginal = context.Set<TEntity>();
    }

    protected string TableName => typeof(TEntity).Name;

    protected DbSet<TEntity> TableOriginal { get; }

    protected virtual IQueryable<TEntity> Table => TableOriginal;

    protected ApplicationContext Context { get; }

    public virtual async Task<TEntity?> GetByIdAsync(TKey id)
    {
        return await Table.FirstOrDefaultAsync(x => Equals(x.Id, id)).ConfigureAwait(false);
    }

    public virtual async Task<TEntity?> GetByIdAsync<TProperty>(TKey id, Expression<Func<TEntity, TProperty>> include)
    {
        return await Table.Include(include).FirstOrDefaultAsync(x => Equals(x.Id, id)).ConfigureAwait(false);
    }

    public virtual async Task<TEntity?> GetByExpressionAsync(Expression<Func<TEntity, bool>> where)
    {
        return await Table.FirstOrDefaultAsync(where).ConfigureAwait(false);
    }

    public virtual async Task<TEntity?> GetByExpressionAsync<TProperty>(Expression<Func<TEntity, bool>> where, Expression<Func<TEntity, TProperty>> include)
    {
        return await Table.Include(include).FirstOrDefaultAsync(where).ConfigureAwait(false);
    }

    public virtual async Task<IEnumerable<TEntity>> GetRangeByIdsAsync(IEnumerable<TKey> ids, bool asNoTracking = false)
    {
        var queryable = Table;
        if (asNoTracking)
        {
            queryable = queryable.AsNoTracking();
        }

        return await queryable.Where(x => ids.Contains(x.Id)).ToListAsync().ConfigureAwait(false);
    }

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync(bool asNoTracking = false)
    {
        var queryable = Table;
        if (asNoTracking)
        {
            queryable = queryable.AsNoTracking();
        }

        return await queryable.ToListAsync().ConfigureAwait(false);
    }

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync<TProperty>(Expression<Func<TEntity, TProperty>> include)
    {
        return await Table.Include(include).ToListAsync().ConfigureAwait(false);
    }

    public virtual async Task<List<TEntity>> GetAllWithExpressionAsync(Expression<Func<TEntity, bool>> @where, bool asNoTracking = false)
    {
        var queryable = Table.Where(where);
        if (asNoTracking)
        {
            queryable = queryable.AsNoTracking();
        }

        return await queryable.ToListAsync().ConfigureAwait(false);
    }

    public virtual async Task<IEnumerable<TEntity>> GetAllWithExpressionAsync<TProperty>(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, TProperty>>[] includes)
    {
        var table = includes.Aggregate(Table, (current, include) => current.Include(include));
        return await table.Where(where).ToListAsync().ConfigureAwait(false);
    }

    public virtual async Task<IEnumerable<TEntity>> GetAllWithExpressionAndLimitAsync(Expression<Func<TEntity, bool>> where, int limit)
    {
        return await Table.Where(where).Take(limit).ToListAsync().ConfigureAwait(false);
    }

    public virtual async Task RemoveRangeAsync(List<TEntity> entities)
    {
        Context.Set<TEntity>().RemoveRange(entities);
        await Context.SaveChangesAsync().ConfigureAwait(false);
    }

    public virtual async Task ExecuteDeleteAsync(Expression<Func<TEntity, bool>> expression)
    {
        await TableOriginal.Where(expression).ExecuteDeleteAsync();
        await Context.SaveChangesAsync().ConfigureAwait(false);
    }

    public async Task RemoveAllAsync()
    {
        await TableOriginal.ExecuteDeleteAsync();

        await ResetIdAsync();
    }

    public async Task ResetIdAsync()
    {
        var tableName = TableOriginal.EntityType.GetTableName();
        var parameter = new SqlParameter("@tableName", tableName);
        const string Sql = "ALTER SEQUENCE public.\"@tableName\" RESTART WITH 1";
        await Context.Database.ExecuteSqlRawAsync(Sql, parameter);
        await Context.SaveChangesAsync().ConfigureAwait(false);
    }

    public void ChangeTrackerClear()
    {
        Context.ChangeTracker.Clear();
    }

    public virtual async Task DeleteAsync(TEntity entity)
    {
        Context.Set<TEntity>().Attach(entity);
        Context.Entry(entity).State = EntityState.Deleted;
        Context.Set<TEntity>().Remove(entity);
        await Context.SaveChangesAsync().ConfigureAwait(false);
    }

    public virtual async Task AddAsync(TEntity row)
    {
        await TableOriginal.AddAsync(row).ConfigureAwait(false);
        await Context.SaveChangesAsync().ConfigureAwait(false);
    }

    public virtual async Task AddRangeAsync(IEnumerable<TEntity> rows)
    {
        var domainModels = rows.ToList();
        await TableOriginal.AddRangeAsync(domainModels);
        await Context.SaveChangesAsync().ConfigureAwait(false);
    }

    public virtual async Task ModifyAsync(TEntity row)
    {
        if (row == null)
        {
            throw new NullReferenceException();
        }

        var isExist = await IsExistWithExpressionAsync(x => Equals(x.Id, row.Id)).ConfigureAwait(false);

        if (isExist)
        {
            TableOriginal.Update(row);

            await Context.SaveChangesAsync().ConfigureAwait(false);
        }
    }

    public virtual async Task ModifyWithPropertiesWithoutModifyAsync<TProperty>(TEntity row, params Expression<Func<TEntity, TProperty>>[] propertiesWithoutModify)
    {
        if (row == null || propertiesWithoutModify == null)
        {
            throw new NullReferenceException();
        }

        var isExist = await IsExistWithExpressionAsync(x => Equals(x.Id, row.Id)).ConfigureAwait(false);

        if (isExist)
        {
            TableOriginal.Update(row);

            foreach (var propertyWithoutModify in propertiesWithoutModify)
            {
                Context.Entry(row).Property(propertyWithoutModify).IsModified = false;
            }

            await Context.SaveChangesAsync().ConfigureAwait(false);
        }
    }

    public virtual async Task ModifyRangeAsync(IEnumerable<TEntity> rows)
    {
        TableOriginal.UpdateRange(rows);
        await Context.SaveChangesAsync().ConfigureAwait(false);
    }

    public virtual async Task<bool> IsExistWithExpressionAsync(Expression<Func<TEntity, bool>> where)
    {
        return await Table.AnyAsync(where).ConfigureAwait(false);
    }

    public virtual async Task<bool> IsExistByIdAsync(TKey id)
    {
        return await Table.AnyAsync(x => Equals(x.Id, id)).ConfigureAwait(false);
    }

    public async Task SaveChangesAsync()
    {
        await Context.SaveChangesAsync().ConfigureAwait(false);
    }

    public virtual bool IsExistWithExpression(Expression<Func<TEntity, bool>> where)
    {
        return Table.Any(where);
    }
}