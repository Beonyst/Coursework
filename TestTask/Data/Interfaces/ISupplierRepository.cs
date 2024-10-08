#nullable enable
using API.Data.Models;

namespace API.Data.Interfaces;

public interface ISupplierRepository : IGenericRepository<Supplier>
{
    Supplier? CreateAndAdd(string name);

    void Update(Supplier supplier, string name);
}
