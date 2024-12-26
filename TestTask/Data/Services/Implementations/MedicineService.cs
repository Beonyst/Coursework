// Класс MedicineService реализует интерфейс IMedicineService и предоставляет методы для работы с медикаментами,
// включая получение, добавление, обновление и удаление медикаментов.
#nullable enable
using API.Data.Interfaces;
using API.Data.Models;
using API.Data.Services.Interfaces;

namespace API.Data.Services.Implementations;

public class MedicineService : IMedicineService
{
    private readonly IMedicineRepository _medicineRepository;
    private readonly ISupplierRepository _supplierRepository;
    private readonly IUnitOfWork _unitOfWork;

    // Конструктор принимает репозиторий медикаментов, репозиторий поставщиков и единицу работы для инициализации сервисных операций.
    public MedicineService(IMedicineRepository medicineRepository, ISupplierRepository supplierRepository, IUnitOfWork unitOfWork)
    {
        _medicineRepository = medicineRepository;
        _supplierRepository = supplierRepository;
        _unitOfWork = unitOfWork;
    }

    // Асинхронное получение медикамента по идентификатору
    public async Task<Medicine> GetAsync(int id)
    {
        var medicine = await _medicineRepository.GetByIdAsync(id);

        if (medicine is null)
        {
            // Генерация исключения, если медикамент не найден
            throw new Exception($"Медикамент с идентификатором {id} не найден");
        }

        return medicine;
    }

    // Получение медикамента по идентификатору синхронно
    public Medicine Get(int id)
    {
        var medicine = _medicineRepository.GetById(id);

        if (medicine is null)
        {
            // Генерация исключения, если медикамент не найден
            throw new Exception($"Медицинский товар с идентификатором {id} не найден");
        }

        return medicine;
    }

    // Получение всех медикаментов
    public Medicine[] GetAll()
    {
        return _medicineRepository.GetAll().ToArray();
    }

    // Обновление информации о медикаменте
    public void UpdateMedicine(int id, string name, string description, double price, int supplierId)
    {
        var medicineToUpdate = Get(id);
        var supplier = _supplierRepository.GetById(supplierId);
        
        if (supplier is null)
        {
            // Генерация исключения, если поставщик не найден
            throw new Exception($"Не найден поставщик с идентификатором {supplierId}");
        }

        // Обновление информации о медикаменте
        _medicineRepository.Update(medicineToUpdate, name, description, price, supplier);
        _unitOfWork.Commit(); // Сохранение изменений
    }

    // Добавление нового медикамента
    public Medicine AddMedicine(string name, string description, double price, int supplierId)
    {
        var supplier = _supplierRepository.GetById(supplierId);
        
        if (supplier is null)
        {
            // Генерация исключения, если поставщик не найден
            throw new Exception($"Не найден поставщик с идентификатором {supplierId}");
        }

        // Создание и добавление нового медикамента
        var medicine = _medicineRepository.CreateAndAdd(name, description, price, supplier);

        if (medicine is null)
        {
            // Генерация исключения, если медикамент с таким наименованием и поставщиком уже существует
            throw new Exception("Товар с таким наименованием и поставщиком уже создан");
        }

        _unitOfWork.Commit(); // Сохранение изменений

        return medicine;
    }

    // Удаление медикамента по идентификатору
    public void DeleteMedicine(int id)
    {
        var medicineToDelete = Get(id);

        // Удаление медикамента
        _medicineRepository.Delete(medicineToDelete);
        _unitOfWork.Commit(); // Сохранение изменений
    }
}
