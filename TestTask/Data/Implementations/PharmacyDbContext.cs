// Класс PharmacyDbContext, который представляет контекст базы данных для работы с сущностями Medicines и Suppliers.

#nullable enable
using API.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Implementations;

// Контекст базы данных для работы с сущностями "Лекарства" (Medicine) и "Поставщики" (Supplier).
public class PharmacyDbContext : DbContext
{
    // Конструктор контекста базы данных без параметров.
    public PharmacyDbContext()
    {
    }

    // Свойства для доступа к таблицам "Medicines" и "Suppliers" в базе данных.
    public virtual DbSet<Medicine> Medicines { get; set; }
    public virtual DbSet<Supplier> Suppliers { get; set; }

    // Метод для конфигурации модели данных с помощью Fluent API.
    // Здесь устанавливается связь "один ко многим" между поставщиками и лекарствами.
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Supplier>()
            // Указывает, что один поставщик может иметь много лекарств.
            .HasMany(s => s.Medicines)
            // Указывает, что каждое лекарство связано с одним поставщиком.
            .WithOne(m => m.Supplier)
            // Устанавливает внешний ключ для связи с поставщиком.
            .HasForeignKey(m => m.SupplierId);
    }

    // Метод для конфигурации источника данных.
    // В данном случае используется SQLite для подключения к базе данных "pharmacy.db".
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=Data/pharmacy.db");
    }
}
