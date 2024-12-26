// Миграция DeleteIsObsolete удаляет столбец "IsObsolete" из таблицы "Suppliers" в базе данных.
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class DeleteIsObsolete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Удаление столбца "IsObsolete" из таблицы "Suppliers"
            migrationBuilder.DropColumn(
                name: "IsObsolete",
                table: "Suppliers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Восстановление столбца "IsObsolete" в таблице "Suppliers", если миграция будет отменена
            migrationBuilder.AddColumn<bool>(
                name: "IsObsolete",
                table: "Suppliers",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }
    }
}
