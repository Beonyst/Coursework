#nullable enable
using API.Data.Models;  

namespace API.Data.Services.Interfaces;

public interface ISupplierService : IService
{
    Task<Supplier> GetAsync(int id);

    Supplier Get(int id);

    Supplier[] GetAll();

    void UpdateSupplier(int id, string name);

    Supplier AddSupplier(string name);

    void DeleteSupplier(int id);
}
