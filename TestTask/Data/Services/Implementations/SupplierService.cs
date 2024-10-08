#nullable enable
using API.Data.Interfaces;
using API.Data.Models;
using API.Data.Services.Interfaces;

namespace API.Data.Services.Implementations;

public class SupplierService : ISupplierService
{
    private readonly ISupplierRepository _supplierRepository;
    private readonly IUnitOfWork _unitOfWork;

    public SupplierService(ISupplierRepository supplierRepository, IUnitOfWork unitOfWork)
    {
        _supplierRepository = supplierRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Supplier> GetAsync(int id)
    {
        var supplier = await _supplierRepository.GetByIdAsync(id);

        if (supplier is null)
        {
            throw new Exception($"Поставщик с идентификатором {id} не найден");
        }

        return supplier;
    }

    public Supplier Get(int id)
    {
        var supplier = _supplierRepository.GetById(id);

        if (supplier is null)
        {
            throw new Exception($"Поставщик с идентификатором {id} не найден");
        }

        return supplier;
    }


    public Supplier[] GetAll()
    {
        return _supplierRepository.GetAll().ToArray();
    }

    public void UpdateSupplier(int id, string name)
    {
        var supplierToUpdate = Get(id);

        _supplierRepository.Update(supplierToUpdate, name);
        _unitOfWork.Commit();
    }

    public Supplier AddSupplier(string name)
    {
        var supplier = _supplierRepository.CreateAndAdd(name);

        if (supplier is null)
        {
            throw new Exception("Поставщик с таким наименованием уже создан");
        }

        _unitOfWork.Commit();

        return supplier;
    }

    public void DeleteSupplier(int id)
    {
        var supplierToDelete = Get(id);

        _supplierRepository.Delete(supplierToDelete);
        _unitOfWork.Commit();
    }
}
