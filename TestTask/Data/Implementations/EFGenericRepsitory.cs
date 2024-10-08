#nullable enable
using API.Data.Interfaces;
using API.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace API.Data.Implementations;

public class EFGenericRepsitory<TEntity> : IGenericRepository<TEntity> where TEntity : Entity
{
    protected readonly PharmacyDbContext _dbContext;
    protected readonly DbSet<TEntity> _dbSet;

    public EFGenericRepsitory(PharmacyDbContext dbContext)
    {
        _dbContext = dbContext;
        _dbSet = _dbContext.Set<TEntity>();
    }

    protected IQueryable<TEntity> GetQueryable(Expression<Func<TEntity, bool>>? filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>[]? includes = null, int? skip = null, int? take = null)

    {
        IQueryable<TEntity> query = _dbSet;

        if (filter is not null)
        {
            query = query.Where(filter);
        }

        if (orderBy is not null)
        {
            query = orderBy(query);
        }

        if (includes is not null)
        {
            query = includes.Aggregate(query, (current, include) => include(current));
        }

        if (skip is not null)
        {
            query = query.Skip(skip.Value);
        }

        if (take is not null)
        {
            query = query.Take(take.Value);
        }

        return query;
    }

    public TEntity? GetById(object? id)
    {
        return id is null ? null : _dbSet.Find(id);
    }

    public Task<TEntity?> GetByIdAsync(object? id)
    {
        return id is null ? Task.FromResult((TEntity?)null) : _dbSet.FindAsync(id).AsTask();
    }

    public virtual IEnumerable<TEntity> GetAll()
    {
        var entities = GetQueryable().AsEnumerable();
        return entities;
    }

    public IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>>? filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>[]? includes = null, int? skip = null, int? take = null)
    {
        return GetQueryable(filter, orderBy, includes, skip, take).ToList();
    }

    public int GetCount(Expression<Func<TEntity, bool>>? predicate = null)
    {
        return GetQueryable(predicate).Count();
    }

    public TEntity Add(TEntity entity)
    {
        return _dbSet.Add(entity).Entity;
    }

    public void Update(TEntity entity)
    {
        _dbContext.Entry(entity).State = EntityState.Modified;
    }

    public void Delete(TEntity entity)
    {
        if (_dbContext.Entry(entity).State == EntityState.Detached)
        {
            _dbSet.Attach(entity);
        }

        _dbSet.Remove(entity);
    }
}
