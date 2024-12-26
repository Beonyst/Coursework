// Интерфейс ISupplierRepository, который представляет собой репозиторий для работы с сущностью поставщика (Supplier).

#nullable enable
using API.Data.Models;

namespace API.Data.Interfaces;

// Репозиторий для работы с поставщиками, расширяющий общий интерфейс репозитория (IGenericRepository) для сущности Supplier.
public interface ISupplierRepository : IGenericRepository<Supplier>
{
    // Метод для создания и добавления нового поставщика с заданным именем.
    Supplier? CreateAndAdd(string name);

    // Метод для обновления информации о поставщике, изменяя его имя.
    void Update(Supplier supplier, string name);
}
