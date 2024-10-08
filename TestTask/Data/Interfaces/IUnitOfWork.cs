#nullable enable
using API.Data.Models;

namespace API.Data.Interfaces;

public interface IUnitOfWork : IDisposable
{
    int Commit();

    void BulkInsert<TEntity>(IList<TEntity> entities) where TEntity : Entity;
}
