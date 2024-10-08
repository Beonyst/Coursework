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

    public MedicineService(IMedicineRepository medicineRepository, ISupplierRepository supplierRepository, IUnitOfWork unitOfWork)
    {
        _medicineRepository = medicineRepository;
        _supplierRepository = supplierRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Medicine> GetAsync(int id)
    {
        var medicine = await _medicineRepository.GetByIdAsync(id);

        if (medicine is null)
        {
            throw new Exception($"Медикамент с идентификатором {id} не найден");
        }

        return medicine;
    }
    public Medicine Get(int id)
    {
        var medicine = _medicineRepository.GetById(id);

        if (medicine is null)
        {
            throw new Exception($"Медицинский товар с идентификатором {id} не найден");
        }

        return medicine;
    }


    public Medicine[] GetAll()
    {
        return _medicineRepository.GetAll().ToArray();
    }

    public void UpdateMedicine(int id, string name, string description, double price, int supplierId)
    {
        var medicineToUpdate = Get(id);
        var supplier = _supplierRepository.GetById(supplierId);
        if (supplier is null)
        {
            throw new Exception($"Не найден поставщиком с идентификатором {supplierId}");
        }

        _medicineRepository.Update(medicineToUpdate, name, description, price, supplier);
        _unitOfWork.Commit();
    }

    public Medicine AddMedicine(string name, string description, double price, int supplierId)
    {
        var supplier = _supplierRepository.GetById(supplierId);
        if (supplier is null)
        {
            throw new Exception($"Не найден поставщиком с идентификатором {supplierId}");
        }

        var medicine = _medicineRepository.CreateAndAdd(name, description, price, supplier);

        if (medicine is null)
        {
            throw new Exception("Товар с таким наименованием и поставщиком уже создан");
        }

        _unitOfWork.Commit();

        return medicine;
    }

    public void DeleteMedicine(int id)
    {
        var medicineToDelete = Get(id);

        _medicineRepository.Delete(medicineToDelete);
        _unitOfWork.Commit();
    }
}
