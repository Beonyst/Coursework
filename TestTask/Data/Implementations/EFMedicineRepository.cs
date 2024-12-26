// Класс EFMedicineRepository, который реализует репозиторий для работы с сущностью лекарства, расширяя функциональность общего репозитория для сущности Medicine.

#nullable enable
using API.Data.Interfaces;
using API.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Implementations;

// Репозиторий для работы с сущностью Medicine, использующий наследование от общего репозитория для сущности Medicine.
public class EFMedicineRepository : EFGenericRepsitory<Medicine>, IMedicineRepository
{
    // Конструктор, который передает контекст базы данных в базовый репозиторий.
    public EFMedicineRepository(PharmacyDbContext context) : base(context)
    {
    }

    // Метод для получения всех лекарств как IQueryable с включением связанных данных (поставщиков).
    public IQueryable<Medicine> GetAllQueryable()
    {
        return GetQueryable(includes:
        [
            source => source.Include(medicine => medicine.Supplier)
        ]);
    }

    // Переопределенный метод для получения всех лекарств, используя AsSplitQuery для разделения запросов.
    public override Medicine[] GetAll()
    {
        return GetAllQueryable().AsSplitQuery().ToArray();
    }

    // Метод для создания и добавления нового лекарства с указанными параметрами: именем, описанием, ценой и поставщиком.
    // Если такое лекарство уже существует для данного поставщика, возвращает null.
    public Medicine? CreateAndAdd(string name, string description, double price, Supplier supplier)
    {
        // Проверка на наличие лекарства с таким же именем у данного поставщика.
        if (GetCount(medicine => medicine.SupplierId == supplier.Id && medicine.Name == name) > 0)
        {
            return null;
        }

        // Создание нового лекарства.
        var medicine = new Medicine
        {
            Name = name,
            Description = description,
            Price = price,
            Supplier = supplier,
            SupplierId = supplier.Id
        };

        // Добавление лекарства в базу данных.
        Add(medicine);

        // Возвращение добавленного лекарства.
        return medicine;
    }

    // Метод для обновления информации о лекарстве, изменяя его имя, описание, цену и поставщика.
    public void Update(Medicine medicine, string name, string description, double price, Supplier supplier)
    {
        // Обновление информации о лекарстве.
        medicine.Name = name;
        medicine.Description = description;
        medicine.Price = price;
        medicine.Supplier = supplier;

        // Обновление лекарства в базе данных.
        Update(medicine);
    }
}
