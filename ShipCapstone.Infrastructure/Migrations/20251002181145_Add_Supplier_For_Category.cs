using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShipCapstone.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Add_Supplier_For_Category : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_Supplier_SupplierId",
                table: "Product");

            migrationBuilder.DropIndex(
                name: "IX_Product_SupplierId",
                table: "Product");

            migrationBuilder.AddColumn<Guid>(
                name: "SupplierId",
                table: "Category",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Category_SupplierId",
                table: "Category",
                column: "SupplierId");

            migrationBuilder.AddForeignKey(
                name: "FK_Category_Supplier_SupplierId",
                table: "Category",
                column: "SupplierId",
                principalTable: "Supplier",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Category_Supplier_SupplierId",
                table: "Category");

            migrationBuilder.DropIndex(
                name: "IX_Category_SupplierId",
                table: "Category");

            migrationBuilder.DropColumn(
                name: "SupplierId",
                table: "Category");

            migrationBuilder.CreateIndex(
                name: "IX_Product_SupplierId",
                table: "Product",
                column: "SupplierId");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_Supplier_SupplierId",
                table: "Product",
                column: "SupplierId",
                principalTable: "Supplier",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
