#nullable enable
using API.Data.Interfaces;
using API.Data.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Storage;
using Tanneryd.BulkOperations.EFCore;

namespace API.Data.Implementations;

public class UnitOfWork : IUnitOfWork
{
    private PharmacyDbContext? _dbContext;

    public UnitOfWork(PharmacyDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public int Commit()
    {
        return _dbContext!.SaveChanges();
    }

    public void BulkInsert<TEntity>(IList<TEntity> entities) where TEntity : Entity
    {
        using var transaction = _dbContext!.Database.BeginTransaction();
        try
        {
            var sqlTransaction = (SqlTransaction)transaction.GetDbTransaction();
            _dbContext.BulkInsertAll(entities, sqlTransaction, true);
            transaction.Commit();
        }
        catch (Exception)
        {
            transaction.Rollback();
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
        if (disposing)
        {
            if (_dbContext is not null)
            {
                _dbContext.Dispose();
                _dbContext = null;
            }
        }
    }
}
