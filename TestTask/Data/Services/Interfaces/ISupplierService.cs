// Интерфейс ISupplierService определяет методы для работы с поставщиками, такие как получение, добавление, обновление и удаление поставщиков.
#nullable enable
using API.Data.Models;  

namespace API.Data.Services.Interfaces;

public interface ISupplierService : IService
{
    // Получение поставщика по идентификатору асинхронно
    Task<Supplier> GetAsync(int id);

    // Получение поставщика по идентификатору синхронно
    Supplier Get(int id);

    // Получение всех поставщиков
    Supplier[] GetAll();

    // Обновление информации о поставщике по идентификатору
    void UpdateSupplier(int id, string name);

    // Добавление нового поставщика
    Supplier AddSupplier(string name);

    // Удаление поставщика по идентификатору
    void DeleteSupplier(int id);
}
