using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PurchaseBill.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddPurchaseBills : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Purchase_Bills",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Purchase_Bills", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Purchase_Bill_Items",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PurchaseBillId = table.Column<int>(type: "int", nullable: false),
                    ItemName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LocationCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    BatchName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    StandardCost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    StandardPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    DiscountPercentage = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    TotalCost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalSelling = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Purchase_Bill_Items", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Purchase_Bill_Items_Purchase_Bills_PurchaseBillId",
                        column: x => x.PurchaseBillId,
                        principalTable: "Purchase_Bills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Purchase_Bill_Items_PurchaseBillId",
                table: "Purchase_Bill_Items",
                column: "PurchaseBillId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Purchase_Bill_Items");

            migrationBuilder.DropTable(
                name: "Purchase_Bills");
        }
    }
}
