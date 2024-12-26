// Интерфейс IMedicineService определяет методы для работы с медикаментами, такими как получение, добавление, обновление и удаление медикаментов.
#nullable enable
using API.Data.Models;

namespace API.Data.Services.Interfaces;

public interface IMedicineService : IService
{
    // Получение медикамента по идентификатору синхронно
    Medicine Get(int id);

    // Получение медикамента по идентификатору асинхронно
    Task<Medicine> GetAsync(int id);

    // Получение всех медикаментов
    Medicine[] GetAll();

    // Обновление информации о медикаменте
    void UpdateMedicine(int id, string name, string description, double price, int supplierId);

    // Добавление нового медикамента
    Medicine AddMedicine(string name, string description, double price, int supplierId);

    // Удаление медикамента по идентификатору
    void DeleteMedicine(int id);
}
