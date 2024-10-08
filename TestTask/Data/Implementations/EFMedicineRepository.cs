#nullable enable
using API.Data.Interfaces;
using API.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Implementations;

public class EFMedicineRepository : EFGenericRepsitory<Medicine>, IMedicineRepository
{
    public EFMedicineRepository(PharmacyDbContext context) : base(context)
    {
    }

    public IQueryable<Medicine> GetAllQueryable()
    {
        return GetQueryable(includes:
        [
            source => source.Include(medicine => medicine.Supplier)
        ]);
    }

    public override Medicine[] GetAll()
    {
        return GetAllQueryable().AsSplitQuery().ToArray();
    }

    public Medicine? CreateAndAdd(string name, string description, double price, Supplier supplier)
    {
        if (GetCount(medicine => medicine.SupplierId == supplier.Id && medicine.Name == name) > 0)
        {
            return null;
        }

        var medicine = new Medicine
        {
            Name = name,
            Description = description,
            Price = price,
            Supplier = supplier,
            SupplierId = supplier.Id
        };

        Add(medicine);

        return medicine;
    }

    public void Update(Medicine medicine, string name, string description, double price, Supplier supplier)
    {
        medicine.Name = name;
        medicine.Description = description;
        medicine.Price = price;
        medicine.Supplier = supplier;

        Update(medicine);
    }
}
