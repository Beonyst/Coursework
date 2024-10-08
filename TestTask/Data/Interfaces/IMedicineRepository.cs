#nullable enable
using API.Data.Models;

namespace API.Data.Interfaces;

public interface IMedicineRepository : IGenericRepository<Medicine>
{
    Medicine? CreateAndAdd(string name, string description, double price, Supplier supplier);

    void Update(Medicine medicine, string name, string description, double price, Supplier supplier);
}
