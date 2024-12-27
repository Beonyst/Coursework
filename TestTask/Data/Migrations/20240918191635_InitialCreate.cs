using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Создание таблицы "Suppliers" с колонками: Id (автоинкремент) и Name
            migrationBuilder.CreateTable(
                name: "Suppliers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true), // Автоинкрементируемая колонка
                    Name = table.Column<string>(type: "TEXT", nullable: false) // Колонка Name с ненулевым значением
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Suppliers", x => x.Id); // Первичный ключ по колонке Id
                });

            // Создание таблицы "Medicines" с колонками: Id (автоинкремент), Name, Description, Price и SupplierId (внешний ключ)
            migrationBuilder.CreateTable(
                name: "Medicines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true), // Автоинкрементируемая колонка
                    Name = table.Column<string>(type: "TEXT", nullable: false), // Колонка Name с ненулевым значением
                    Description = table.Column<string>(type: "TEXT", nullable: false), // Колонка Description с ненулевым значением
                    Price = table.Column<decimal>(type: "TEXT", nullable: false), // Колонка Price с ненулевым значением
                    SupplierId = table.Column<int>(type: "INTEGER", nullable: false) // Внешний ключ к таблице Supplier
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Medicines", x => x.Id); // Первичный ключ по колонке Id
                    table.ForeignKey(
                        name: "FK_Medicines_Suppliers_SupplierId", // Определение внешнего ключа для связи с таблицей Suppliers
                        column: x => x.SupplierId,
                        principalTable: "Suppliers", // Ссылка на таблицу Suppliers
                        principalColumn: "Id", // Колонка SupplierId ссылается на Id в таблице Suppliers
                        onDelete: ReferentialAction.Cascade); // Каскадное удаление: при удалении поставщика связанные записи лекарств также удаляются
                });

            // Создание индекса на колонке SupplierId в таблице Medicines для ускорения поиска
            migrationBuilder.CreateIndex(
                name: "IX_Medicines_SupplierId",
                table: "Medicines",
                column: "SupplierId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Удаление таблицы "Medicines"
            migrationBuilder.DropTable(
                name: "Medicines");

            // Удаление таблицы "Suppliers"
            migrationBuilder.DropTable(
                name: "Suppliers");
        }
    }
}
