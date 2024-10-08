#nullable enable
using Wpf.Models;

namespace Wpf.Services.Interfaces;

public interface ISupplierService
{
    Task<Supplier> GetAsync(int id);

    Task<Supplier[]> GetAllAsync();

    Task UpdateSupplierAsync(Supplier supplier);

    Task<Supplier> AddSupplierAsync(string name);

    Task DeleteSupplier(int id);
}
