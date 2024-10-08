#nullable enable
using API.Data.Interfaces;
using API.Data.Models;

namespace API.Data.Implementations;

public class EFSupplierRepository : EFGenericRepsitory<Supplier>, ISupplierRepository
{
    public EFSupplierRepository(PharmacyDbContext context) : base(context)
    {
    }

    public Supplier? CreateAndAdd(string name)
    {
        if (GetCount(supplier => supplier.Name == name) > 0)
        {
            return null;
        }

        var supplier = new Supplier
        {
            Name = name,
        };

        Add(supplier);

        return supplier;
    }

    public void Update(Supplier supplier, string name)
    {
        supplier.Name = name;

        Update(supplier);
    }
}
