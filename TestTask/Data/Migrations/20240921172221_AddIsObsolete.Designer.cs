// Этот файл был сгенерирован автоматически для миграции с именем "20240921172221_AddIsObsolete" и представляет описание изменений в модели базы данных.
#nullable disable

using API.Data.Implementations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Data.Migrations
{
    // Класс AddIsObsolete представляет миграцию для добавления нового поля "IsObsolete" в таблицу "Suppliers".
    // Это будет применено к базе данных через Entity Framework.
    [DbContext(typeof(PharmacyDbContext))]
    [Migration("20240921172221_AddIsObsolete")]
    partial class AddIsObsolete
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.8");

            // Определение модели для таблицы "Medicines"
            modelBuilder.Entity("Data.Models.Medicine", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<double>("Price")
                        .HasColumnType("REAL");

                    b.Property<int>("SupplierId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id"); // Установка Id как первичного ключа

                    b.HasIndex("SupplierId"); // Создание индекса для SupplierId

                    b.ToTable("Medicines"); // Указание таблицы, с которой связана сущность
                });

            // Определение модели для таблицы "Suppliers"
            modelBuilder.Entity("Data.Models.Supplier", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsObsolete") // Добавление нового поля "IsObsolete" в таблицу "Suppliers"
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id"); // Установка Id как первичного ключа

                    b.ToTable("Suppliers"); // Указание таблицы, с которой связана сущность
                });

            // Определение связи между сущностями Medicine и Supplier
            modelBuilder.Entity("Data.Models.Medicine", b =>
                {
                    b.HasOne("Data.Models.Supplier", "Supplier")
                        .WithMany("Medicines") // Связь один ко многим
                        .HasForeignKey("SupplierId") // Связь по SupplierId
                        .OnDelete(DeleteBehavior.Cascade) // Удаление каскадом при удалении поставщика
                        .IsRequired();

                    b.Navigation("Supplier");
                });

            modelBuilder.Entity("Data.Models.Supplier", b =>
                {
                    b.Navigation("Medicines"); // Навигация по коллекции медикаментов, связанных с поставщиком
                });
#pragma warning restore 612, 618
        }
    }
}
