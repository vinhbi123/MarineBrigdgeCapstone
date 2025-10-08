using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShipCapstone.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Add_Supplier_For_ModifierGroup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "SupplierId",
                table: "ModifierGroup",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_ModifierGroup_SupplierId",
                table: "ModifierGroup",
                column: "SupplierId");

            migrationBuilder.AddForeignKey(
                name: "FK_ModifierGroup_Supplier_SupplierId",
                table: "ModifierGroup",
                column: "SupplierId",
                principalTable: "Supplier",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ModifierGroup_Supplier_SupplierId",
                table: "ModifierGroup");

            migrationBuilder.DropIndex(
                name: "IX_ModifierGroup_SupplierId",
                table: "ModifierGroup");

            migrationBuilder.DropColumn(
                name: "SupplierId",
                table: "ModifierGroup");
        }
    }
}
