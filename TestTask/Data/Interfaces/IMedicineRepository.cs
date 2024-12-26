// Интерфейс IMedicineRepository, который представляет собой репозиторий для работы с сущностью лекарства (Medicine).

#nullable enable
using API.Data.Models;

namespace API.Data.Interfaces;

// Репозиторий для работы с лекарствами, расширяющий общий интерфейс репозитория (IGenericRepository) для сущности Medicine.
public interface IMedicineRepository : IGenericRepository<Medicine>
{
    // Метод для создания и добавления нового лекарства с указанными параметрами: именем, описанием, ценой и поставщиком.
    Medicine? CreateAndAdd(string name, string description, double price, Supplier supplier);

    // Метод для обновления информации о лекарстве, изменяя его имя, описание, цену и поставщика.
    void Update(Medicine medicine, string name, string description, double price, Supplier supplier);
}
