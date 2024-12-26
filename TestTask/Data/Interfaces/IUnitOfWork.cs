// Интерфейс IUnitOfWork, который представляет собой абстракцию для работы с репозиториями и управления транзакциями.

#nullable enable
using API.Data.Models;

namespace API.Data.Interfaces;

// Интерфейс для работы с единицей работы (Unit of Work).
// Он включает методы для фиксации изменений и массовой вставки данных.
public interface IUnitOfWork : IDisposable
{
    // Метод для фиксации изменений в базе данных.
    int Commit();

    // Метод для массовой вставки сущностей в базу данных.
    // Он позволяет вставить несколько объектов типа TEntity, где TEntity является наследником класса Entity.
    void BulkInsert<TEntity>(IList<TEntity> entities) where TEntity : Entity;
}
