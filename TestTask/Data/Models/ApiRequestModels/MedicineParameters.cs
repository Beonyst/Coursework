// Класс MedicineParameters используется для представления параметров медикамента, которые могут быть переданы в запросах API.
#nullable enable

namespace API.Data.Models.ApiRequestModels;

public class MedicineParameters
{
    // Название медикамента
    public string Name { get; set; } = null!;

    // Описание медикамента
    public string Description { get; set; } = null!;

    // Цена медикамента
    public double Price { get; set; }

    // Идентификатор поставщика, связанного с медикаментом
    public int SupplierId { get; set; }
}
