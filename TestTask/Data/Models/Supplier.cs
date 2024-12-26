// Класс Supplier представляет поставщика и включает его свойства, такие как имя и коллекцию связанных медикаментов.
#nullable enable
using System.Text.Json.Serialization;

namespace API.Data.Models;

public class Supplier : Entity
{
    // Название поставщика
    public string Name { get; set; } = null!;

    // Коллекция медикаментов, связанных с данным поставщиком. Это свойство игнорируется при сериализации в JSON.
    [JsonIgnore]
    public virtual ICollection<Medicine> Medicines { get; set; } = null!;
}
