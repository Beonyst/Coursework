// Класс SupplierService реализует интерфейс ISupplierService и предоставляет методы для работы с поставщиками,
// включая получение, добавление, обновление и удаление поставщиков.
#nullable enable
using API.Data.Interfaces;
using API.Data.Models;
using API.Data.Services.Interfaces;

namespace API.Data.Services.Implementations;

public class SupplierService : ISupplierService
{
    private readonly ISupplierRepository _supplierRepository;
    private readonly IUnitOfWork _unitOfWork;

    // Конструктор принимает репозиторий поставщиков и единицу работы для инициализации сервисных операций.
    public SupplierService(ISupplierRepository supplierRepository, IUnitOfWork unitOfWork)
    {
        _supplierRepository = supplierRepository;
        _unitOfWork = unitOfWork;
    }

    // Асинхронное получение поставщика по идентификатору
    public async Task<Supplier> GetAsync(int id)
    {
        var supplier = await _supplierRepository.GetByIdAsync(id);

        if (supplier is null)
        {
            // Генерация исключения, если поставщик не найден
            throw new Exception($"Поставщик с идентификатором {id} не найден");
        }

        return supplier;
    }

    // Получение поставщика по идентификатору синхронно
    public Supplier Get(int id)
    {
        var supplier = _supplierRepository.GetById(id);

        if (supplier is null)
        {
            // Генерация исключения, если поставщик не найден
            throw new Exception($"Поставщик с идентификатором {id} не найден");
        }

        return supplier;
    }

    // Получение всех поставщиков
    public Supplier[] GetAll()
    {
        return _supplierRepository.GetAll().ToArray();
    }

    // Обновление информации о поставщике по идентификатору
    public void UpdateSupplier(int id, string name)
    {
        var supplierToUpdate = Get(id);

        // Обновление информации о поставщике
        _supplierRepository.Update(supplierToUpdate, name);
        _unitOfWork.Commit(); // Сохранение изменений
    }

    // Добавление нового поставщика
    public Supplier AddSupplier(string name)
    {
        var supplier = _supplierRepository.CreateAndAdd(name);

        if (supplier is null)
        {
            // Генерация исключения, если поставщик с таким наименованием уже существует
            throw new Exception("Поставщик с таким наименованием уже создан");
        }

        _unitOfWork.Commit(); // Сохранение изменений

        return supplier;
    }

    // Удаление поставщика по идентификатору
    public void DeleteSupplier(int id)
    {
        var supplierToDelete = Get(id);

        // Удаление поставщика
        _supplierRepository.Delete(supplierToDelete);
        _unitOfWork.Commit(); // Сохранение изменений
    }
}
