using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShipCapstone.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Remove_ProductModifierGroup_And_Add_ProductVariantOption_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductModifierGroup");

            migrationBuilder.CreateTable(
                name: "ProductVariantOption",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductVariantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifierOptionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductVariantOption", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductVariantOption_ModifierOption_ModifierOptionId",
                        column: x => x.ModifierOptionId,
                        principalTable: "ModifierOption",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductVariantOption_ProductVariant_ProductVariantId",
                        column: x => x.ProductVariantId,
                        principalTable: "ProductVariant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductVariantOption_ModifierOptionId",
                table: "ProductVariantOption",
                column: "ModifierOptionId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductVariantOption_ProductVariantId",
                table: "ProductVariantOption",
                column: "ProductVariantId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductVariantOption");

            migrationBuilder.CreateTable(
                name: "ProductModifierGroup",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifierGroupId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductModifierGroup", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductModifierGroup_ModifierGroup_ModifierGroupId",
                        column: x => x.ModifierGroupId,
                        principalTable: "ModifierGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductModifierGroup_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductModifierGroup_ModifierGroupId",
                table: "ProductModifierGroup",
                column: "ModifierGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductModifierGroup_ProductId",
                table: "ProductModifierGroup",
                column: "ProductId");
        }
    }
}
