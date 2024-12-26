// Класс UnitOfWork, который реализует интерфейс IUnitOfWork для работы с транзакциями и сохранением изменений в базе данных.

#nullable enable
using API.Data.Interfaces;
using API.Data.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Storage;
using Tanneryd.BulkOperations.EFCore;

namespace API.Data.Implementations;

// Класс, реализующий единицу работы, которая управляет транзакциями и взаимодействует с контекстом базы данных.
public class UnitOfWork : IUnitOfWork
{
    // Поле для хранения контекста базы данных.
    private PharmacyDbContext? _dbContext;

    // Конструктор, принимающий контекст базы данных для инициализации.
    public UnitOfWork(PharmacyDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    // Метод для фиксации изменений в базе данных.
    public int Commit()
    {
        // Сохраняет все изменения в базе данных и возвращает количество затронутых строк.
        return _dbContext!.SaveChanges();
    }

    // Метод для массовой вставки сущностей в базу данных с использованием транзакции.
    public void BulkInsert<TEntity>(IList<TEntity> entities) where TEntity : Entity
    {
        // Начинает транзакцию.
        using var transaction = _dbContext!.Database.BeginTransaction();
        try
        {
            // Получает SQL транзакцию из контекста базы данных.
            var sqlTransaction = (SqlTransaction)transaction.GetDbTransaction();
            // Выполняет массовую вставку данных с использованием транзакции.
            _dbContext.BulkInsertAll(entities, sqlTransaction, true);
            // Коммитит транзакцию.
            transaction.Commit();
        }
        catch (Exception)
        {
            // В случае ошибки откатывает транзакцию.
            transaction.Rollback();
        }
    }

    // Метод для освобождения ресурсов.
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    // Метод для освобождения ресурсов, поддерживающий вариант с параметром disposing.
    private void Dispose(bool disposing)
    {
        if (disposing)
        {
            // Если контекст базы данных не равен null, его нужно очистить.
            if (_dbContext is not null)
            {
                _dbContext.Dispose();
                _dbContext = null;
            }
        }
    }
}
