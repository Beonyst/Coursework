#nullable enable
using API.Data.Models;

namespace API.Data.Services.Interfaces;

public interface IMedicineService : IService
{
    Medicine Get(int id);

    Task<Medicine> GetAsync(int id);

    Medicine[] GetAll();

    void UpdateMedicine(int id, string name, string description, double price, int supplierId);

    Medicine AddMedicine(string name, string description, double price, int supplierId);

    void DeleteMedicine(int id);
}
