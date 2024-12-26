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
            // Creating the "Suppliers" table with columns: Id (auto-incremented) and Name
            migrationBuilder.CreateTable(
                name: "Suppliers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true), // Auto-incremented column
                    Name = table.Column<string>(type: "TEXT", nullable: false) // Non-nullable Name column
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Suppliers", x => x.Id); // Primary key set on the Id column
                });

            // Creating the "Medicines" table with columns: Id (auto-incremented), Name, Description, Price, and SupplierId (foreign key)
            migrationBuilder.CreateTable(
                name: "Medicines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true), // Auto-incremented column
                    Name = table.Column<string>(type: "TEXT", nullable: false), // Non-nullable Name column
                    Description = table.Column<string>(type: "TEXT", nullable: false), // Non-nullable Description column
                    Price = table.Column<decimal>(type: "TEXT", nullable: false), // Non-nullable Price column
                    SupplierId = table.Column<int>(type: "INTEGER", nullable: false) // Foreign key to the Supplier table
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Medicines", x => x.Id); // Primary key set on the Id column
                    table.ForeignKey(
                        name: "FK_Medicines_Suppliers_SupplierId", // Defining foreign key relationship with the Suppliers table
                        column: x => x.SupplierId,
                        principalTable: "Suppliers", // Referencing the Suppliers table
                        principalColumn: "Id", // The SupplierId column references the Id column of Suppliers
                        onDelete: ReferentialAction.Cascade); // Cascade delete: if a supplier is deleted, related medicines are deleted
                });

            // Creating an index on SupplierId in the Medicines table for faster lookup
            migrationBuilder.CreateIndex(
                name: "IX_Medicines_SupplierId",
                table: "Medicines",
                column: "SupplierId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Dropping the "Medicines" table
            migrationBuilder.DropTable(
                name: "Medicines");

            // Dropping the "Suppliers" table
            migrationBuilder.DropTable(
                name: "Suppliers");
        }
    }
}
