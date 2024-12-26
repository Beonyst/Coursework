// Этот файл был сгенерирован автоматически для миграции с именем "20240922084438_DeleteIsObsolete" и представляет собой описание изменений в модели базы данных.
#nullable disable

using API.Data.Implementations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Data.Migrations
{
    // Класс DeleteIsObsolete представляет миграцию для удаления устаревшего поля или данных.
    // Это будет применено к базе данных через Entity Framework.
    [DbContext(typeof(PharmacyDbContext))]
    [Migration("20240922084438_DeleteIsObsolete")]
    partial class DeleteIsObsolete
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.8");

            // Определение модели для таблицы "Medicines"
            modelBuilder.Entity("API.Data.Models.Medicine", b =>
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
            modelBuilder.Entity("API.Data.Models.Supplier", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id"); // Установка Id как первичного ключа

                    b.ToTable("Suppliers"); // Указание таблицы, с которой связана сущность
                });

            // Определение связи между сущностями Medicine и Supplier
            modelBuilder.Entity("API.Data.Models.Medicine", b =>
                {
                    b.HasOne("API.Data.Models.Supplier", "Supplier")
                        .WithMany("Medicines") // Связь один ко многим
                        .HasForeignKey("SupplierId") // Связь по SupplierId
                        .OnDelete(DeleteBehavior.Cascade) // Удаление каскадом при удалении поставщика
                        .IsRequired();

                    b.Navigation("Supplier");
                });

            modelBuilder.Entity("API.Data.Models.Supplier", b =>
                {
                    b.Navigation("Medicines"); // Навигация по коллекции медикаментов, связанных с поставщиком
                });
#pragma warning restore 612, 618
        }
    }
}
