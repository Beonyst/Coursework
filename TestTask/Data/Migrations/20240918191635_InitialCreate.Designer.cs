// Миграция InitialCreate создает начальную структуру базы данных для таблиц "Medicines" и "Suppliers", включая связи между ними.
using API.Data.Implementations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Data.Migrations
{
    [DbContext(typeof(PharmacyDbContext))]
    [Migration("20240918191635_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            // Установка версии продукта для миграции
            modelBuilder.HasAnnotation("ProductVersion", "8.0.8");

            // Определение таблицы "Medicines"
            modelBuilder.Entity("Data.Models.Medicine", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    // Столбец "Description" обязателен для заполнения и имеет тип данных "TEXT"
                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    // Столбец "Name" обязателен для заполнения и имеет тип данных "TEXT"
                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    // Столбец "Price" имеет тип данных "TEXT", но для хранения числовых значений должен быть изменен в миграции
                    b.Property<decimal>("Price")
                        .HasColumnType("TEXT");

                    // Столбец "SupplierId" ссылается на таблицу "Suppliers"
                    b.Property<int>("SupplierId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id"); // Установка первичного ключа

                    // Создание индекса для внешнего ключа "SupplierId" для улучшения производительности
                    b.HasIndex("SupplierId");

                    b.ToTable("Medicines"); // Таблица "Medicines"
                });

            // Определение таблицы "Suppliers"
            modelBuilder.Entity("Data.Models.Supplier", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    // Столбец "Name" обязателен для заполнения и имеет тип данных "TEXT"
                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id"); // Установка первичного ключа

                    b.ToTable("Suppliers"); // Таблица "Suppliers"
                });

            // Определение связи между таблицами "Medicines" и "Suppliers"
            modelBuilder.Entity("Data.Models.Medicine", b =>
                {
                    b.HasOne("Data.Models.Supplier", "Supplier") // Связь "Medicine" с "Supplier"
                        .WithMany("Medicines") // Один поставщик может иметь несколько медикаментов
                        .HasForeignKey("SupplierId") // Внешний ключ "SupplierId"
                        .OnDelete(DeleteBehavior.Cascade) // При удалении поставщика, все связанные медикаменты также будут удалены
                        .IsRequired(); // Внешний ключ обязателен

                    b.Navigation("Supplier"); // Навигация к связанному объекту Supplier
                });

            // Навигация для коллекции медикаментов в сущности Supplier
            modelBuilder.Entity("Data.Models.Supplier", b =>
                {
                    b.Navigation("Medicines"); // Связь поставщика с его медикаментами
                });
#pragma warning restore 612, 618
        }
    }
}
