#nullable enable

namespace API.Data.Models;

public class Medicine : Entity
{
    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public double Price { get; set; }

    public int SupplierId { get; set; }

    public virtual Supplier Supplier { get; set; } = null!;
}
