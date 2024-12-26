#nullable enable
using Wpf.Models;

namespace Wpf.Services.Interfaces;

// Интерфейс для сервиса работы с поставщиками
public interface ISupplierService
{
    // Асинхронное получение поставщика по его ID
    Task<Supplier> GetAsync(int id);

    // Асинхронное получение всех поставщиков
    Task<Supplier[]> GetAllAsync();

    // Асинхронное обновление информации о поставщике
    Task UpdateSupplierAsync(Supplier supplier);

    // Асинхронное добавление нового поставщика по имени
    Task<Supplier> AddSupplierAsync(string name);

    // Асинхронное удаление поставщика по его ID
    Task DeleteSupplier(int id);
}
