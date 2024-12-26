// Абстрактный класс Entity реализует интерфейс IEntity и представляет базовую сущность с уникальным идентификатором.
#nullable enable
using System.ComponentModel.DataAnnotations;

namespace API.Data.Models;

public abstract class Entity : IEntity
{
    // Уникальный идентификатор сущности, помечен как ключ для базы данных
    [Key]
    public virtual int Id { get; set; }
}
