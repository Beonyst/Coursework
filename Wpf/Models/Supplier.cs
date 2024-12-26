#nullable enable

namespace Wpf.Models;

// Класс, представляющий поставщика лекарства.
public class Supplier
{
    // Уникальный идентификатор поставщика
    public int Id { get; set; }

    // Название поставщика
    public string Name { get; set; } = null!;
}
