// Класс Medicine представляет медикамент и включает его свойства, такие как имя, описание, цена и связанный поставщик.
#nullable enable

namespace API.Data.Models;

public class Medicine : Entity
{
    // Название медикамента
    public string Name { get; set; } = null!;

    // Описание медикамента
    public string Description { get; set; } = null!;

    // Цена медикамента
    public double Price { get; set; }

    // Идентификатор поставщика, связанного с медикаментом
    public int SupplierId { get; set; }

    // Поставщик, связанный с данным медикаментом
    public virtual Supplier Supplier { get; set; } = null!;
}
