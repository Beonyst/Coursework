#nullable enable

namespace Wpf.Models;

// Класс, представляющий лекарство с его основными характеристиками.
public class Medicine
{
    // Уникальный идентификатор лекарства
    public int Id { get; set; }

    // Название лекарства
    public string Name { get; set; } = null!;

    // Описание лекарства
    public string Description { get; set; } = null!;

    // Цена лекарства
    public double Price { get; set; }

    // Поставщик лекарства
    public Supplier Supplier { get; set; } = null!;
}
