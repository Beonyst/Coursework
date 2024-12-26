// Миграция AddIsObsolete добавляет столбец "IsObsolete" в таблицу "Suppliers" и изменяет тип данных столбца "Price" в таблице "Medicines".
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class AddIsObsolete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Добавление нового столбца "IsObsolete" в таблицу "Suppliers" с типом данных "INTEGER" (эквивалент булевого значения)
            migrationBuilder.AddColumn<bool>(
                name: "IsObsolete",
                table: "Suppliers",
                type: "INTEGER",
                nullable: false,
                defaultValue: false); // Столбец не может быть null и по умолчанию имеет значение false

            // Изменение типа данных столбца "Price" в таблице "Medicines" с "TEXT" на "REAL" (для работы с вещественными числами)
            migrationBuilder.AlterColumn<double>(
                name: "Price",
                table: "Medicines",
                type: "REAL",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "TEXT");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Удаление столбца "IsObsolete" из таблицы "Suppliers", если миграция будет отменена
            migrationBuilder.DropColumn(
                name: "IsObsolete",
                table: "Suppliers");

            // Восстановление типа данных столбца "Price" в таблице "Medicines" с "REAL" на "TEXT"
            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Medicines",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "REAL");
        }
    }
}
