// Класс EFSupplierRepository, который реализует репозиторий для работы с поставщиками, расширяя функциональность репозитория для сущности Supplier.

#nullable enable
using API.Data.Interfaces;
using API.Data.Models;

namespace API.Data.Implementations;

// Репозиторий для работы с сущностью Supplier, использующий наследование от общего репозитория для сущности Supplier.
public class EFSupplierRepository : EFGenericRepsitory<Supplier>, ISupplierRepository
{
    // Конструктор, который передает контекст базы данных в базовый репозиторий.
    public EFSupplierRepository(PharmacyDbContext context) : base(context)
    {
    }

    // Метод для создания и добавления нового поставщика с указанным именем.
    // Если поставщик с таким именем уже существует, возвращает null.
    public Supplier? CreateAndAdd(string name)
    {
        // Проверка на наличие поставщика с таким же именем в базе данных.
        if (GetCount(supplier => supplier.Name == name) > 0)
        {
            return null;
        }

        // Создание нового поставщика.
        var supplier = new Supplier
        {
            Name = name,
        };

        // Добавление нового поставщика в базу данных.
        Add(supplier);

        // Возвращение добавленного поставщика.
        return supplier;
    }

    // Метод для обновления информации о поставщике, изменяя его имя.
    public void Update(Supplier supplier, string name)
    {
        // Изменение имени поставщика.
        supplier.Name = name;

        // Обновление информации о поставщике в базе данных.
        Update(supplier);
    }
}
