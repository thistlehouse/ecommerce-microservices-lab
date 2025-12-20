using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inventories.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigrations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Inventories");

            migrationBuilder.CreateTable(
                name: "StockItem",
                schema: "Inventories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Units = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockItem", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StockItem_Id",
                schema: "Inventories",
                table: "StockItem",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StockItem",
                schema: "Inventories");
        }
    }
}
