using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShipCapstone.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Add_Supplier_And_Boatyard_For_Transaction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "BoatyardId",
                table: "Transaction",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SupplierId",
                table: "Transaction",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_BoatyardId",
                table: "Transaction",
                column: "BoatyardId");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_SupplierId",
                table: "Transaction",
                column: "SupplierId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_Boatyard_BoatyardId",
                table: "Transaction",
                column: "BoatyardId",
                principalTable: "Boatyard",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_Supplier_SupplierId",
                table: "Transaction",
                column: "SupplierId",
                principalTable: "Supplier",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_Boatyard_BoatyardId",
                table: "Transaction");

            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_Supplier_SupplierId",
                table: "Transaction");

            migrationBuilder.DropIndex(
                name: "IX_Transaction_BoatyardId",
                table: "Transaction");

            migrationBuilder.DropIndex(
                name: "IX_Transaction_SupplierId",
                table: "Transaction");

            migrationBuilder.DropColumn(
                name: "BoatyardId",
                table: "Transaction");

            migrationBuilder.DropColumn(
                name: "SupplierId",
                table: "Transaction");
        }
    }
}
