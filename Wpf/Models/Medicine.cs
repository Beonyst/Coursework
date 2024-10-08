#nullable enable

namespace Wpf.Models;

public class Medicine
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public double Price { get; set; }

    public Supplier Supplier { get; set; } = null!;
}
