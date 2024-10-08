#nullable enable

namespace API.Data.Models.ApiRequestModels;

public class MedicineParameters
{
    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public double Price { get; set; }

    public int SupplierId { get; set; }
}
