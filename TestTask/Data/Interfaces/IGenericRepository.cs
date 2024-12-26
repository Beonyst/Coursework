// Интерфейс IGenericRepository, который представляет собой общий репозиторий для работы с сущностями в базе данных.

#nullable enable
using API.Data.Models;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace API.Data.Interfaces;

// Репозиторий общего назначения для работы с сущностями типа TEntity, где TEntity наследует от класса Entity.
public interface IGenericRepository<TEntity> where TEntity : Entity
{
    // Метод для получения сущности по идентификатору.
    TEntity? GetById(object? id);

    // Асинхронный метод для получения сущности по идентификатору.
    Task<TEntity?> GetByIdAsync(object? id);

    // Метод для получения всех сущностей.
    IEnumerable<TEntity> GetAll();

    // Метод для получения сущностей с возможностью фильтрации, сортировки и включения связанных данных.
    IEnumerable<TEntity> Get(
        Expression<Func<TEntity, bool>>? filter = null, // Фильтр для поиска
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, // Функция сортировки
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>[]? includes = null, // Включение связанных данных
        int? skip = null, // Пропуск записей
        int? take = null // Ограничение количества записей
    );

    // Метод для получения количества сущностей, соответствующих условию.
    int GetCount(Expression<Func<TEntity, bool>>? predicate = null);

    // Метод для добавления сущности в репозиторий.
    TEntity Add(TEntity entity);

    // Метод для обновления сущности в репозитории.
    void Update(TEntity entity);

    // Метод для удаления сущности из репозитория.
    void Delete(TEntity entity);
}
