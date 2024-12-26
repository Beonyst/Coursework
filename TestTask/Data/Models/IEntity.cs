// Интерфейс IEntity представляет сущность с уникальным идентификатором.
#nullable enable

namespace API.Data.Models;

public interface IEntity
{
    // Уникальный идентификатор сущности
    int Id { get; set; }
}
