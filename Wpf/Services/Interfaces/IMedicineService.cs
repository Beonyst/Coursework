#nullable enable
using Wpf.Models;
using Wpf.Models.ApiRequestModels;

namespace Wpf.Services.Interfaces;

// Интерфейс для сервиса работы с медикаментами
public interface IMedicineService
{
    // Асинхронное получение медикамента по его ID
    Task<Medicine> GetAsync(int id);

    // Асинхронное получение всех медикаментов
    Task<Medicine[]> GetAllAsync();

    // Асинхронное обновление информации о медикаменте по ID с параметрами
    Task UpdateMedicineAsync(int id, MedicineParameters parameters);

    // Асинхронное добавление нового медикамента с параметрами
    Task<Medicine> AddMedicineAsync(MedicineParameters parameters);

    // Асинхронное удаление медикамента по его ID
    Task DeleteMedicine(int id);
}
