#nullable enable

namespace Wpf.Models.ApiRequestModels;

// Класс, представляющий параметры лекарства для API запроса.
public class MedicineParameters
{
    // Название лекарства
    public string Name { get; set; } = null!;

    // Описание лекарства
    public string Description { get; set; } = null!;

    // Цена лекарства
    public double Price { get; set; }

    // Идентификатор поставщика
    public int SupplierId { get; set; }
}
