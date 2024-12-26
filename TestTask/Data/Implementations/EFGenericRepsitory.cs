// Класс EFGenericRepsitory, который реализует общий репозиторий для работы с сущностями в базе данных, используя Entity Framework.

#nullable enable
using API.Data.Interfaces;
using API.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace API.Data.Implementations;

// Общий репозиторий для работы с сущностями типа TEntity, где TEntity является наследником класса Entity.
public class EFGenericRepsitory<TEntity> : IGenericRepository<TEntity> where TEntity : Entity
{
    // Контекст базы данных, используемый для работы с таблицами.
    protected readonly PharmacyDbContext _dbContext;

    // Набор данных для работы с сущностями типа TEntity.
    protected readonly DbSet<TEntity> _dbSet;

    // Конструктор, инициализирующий контекст базы данных и набор данных.
    public EFGenericRepsitory(PharmacyDbContext dbContext)
    {
        _dbContext = dbContext;
        _dbSet = _dbContext.Set<TEntity>();
    }

    // Метод для построения запроса для получения сущностей с возможностью фильтрации, сортировки, включения связанных данных и ограничения количества.
    protected IQueryable<TEntity> GetQueryable(Expression<Func<TEntity, bool>>? filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>[]? includes = null, int? skip = null, int? take = null)
    {
        IQueryable<TEntity> query = _dbSet;

        // Применение фильтра, если он задан.
        if (filter is not null)
        {
            query = query.Where(filter);
        }

        // Применение сортировки, если она задана.
        if (orderBy is not null)
        {
            query = orderBy(query);
        }

        // Включение связанных данных, если они заданы.
        if (includes is not null)
        {
            query = includes.Aggregate(query, (current, include) => include(current));
        }

        // Пропуск записей, если это необходимо.
        if (skip is not null)
        {
            query = query.Skip(skip.Value);
        }

        // Ограничение количества записей, если это необходимо.
        if (take is not null)
        {
            query = query.Take(take.Value);
        }

        return query;
    }

    // Метод для получения сущности по идентификатору.
    public TEntity? GetById(object? id)
    {
        return id is null ? null : _dbSet.Find(id);
    }

    // Асинхронный метод для получения сущности по идентификатору.
    public Task<TEntity?> GetByIdAsync(object? id)
    {
        return id is null ? Task.FromResult((TEntity?)null) : _dbSet.FindAsync(id).AsTask();
    }

    // Метод для получения всех сущностей.
    public virtual IEnumerable<TEntity> GetAll()
    {
        var entities = GetQueryable().AsEnumerable();
        return entities;
    }

    // Метод для получения сущностей с возможностью фильтрации, сортировки и включения связанных данных.
    public IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>>? filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>[]? includes = null, int? skip = null, int? take = null)
    {
        return GetQueryable(filter, orderBy, includes, skip, take).ToList();
    }

    // Метод для получения количества сущностей, соответствующих заданному условию.
    public int GetCount(Expression<Func<TEntity, bool>>? predicate = null)
    {
        return GetQueryable(predicate).Count();
    }

    // Метод для добавления новой сущности в базу данных.
    public TEntity Add(TEntity entity)
    {
        return _dbSet.Add(entity).Entity;
    }

    // Метод для обновления сущности в базе данных.
    public void Update(TEntity entity)
    {
        _dbContext.Entry(entity).State = EntityState.Modified;
    }

    // Метод для удаления сущности из базы данных.
    public void Delete(TEntity entity)
    {
        // Если сущность отсоединена, подключаем её к контексту.
        if (_dbContext.Entry(entity).State == EntityState.Detached)
        {
            _dbSet.Attach(entity);
        }

        // Удаление сущности.
        _dbSet.Remove(entity);
    }
}
