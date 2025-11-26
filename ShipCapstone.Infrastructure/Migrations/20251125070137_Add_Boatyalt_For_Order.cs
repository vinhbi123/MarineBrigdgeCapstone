using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShipCapstone.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Add_Boatyalt_For_Order : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "ShipId",
                table: "Order",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "BoatyardId",
                table: "Order",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Order_BoatyardId",
                table: "Order",
                column: "BoatyardId");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Boatyard_BoatyardId",
                table: "Order",
                column: "BoatyardId",
                principalTable: "Boatyard",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_Boatyard_BoatyardId",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Order_BoatyardId",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "BoatyardId",
                table: "Order");

            migrationBuilder.AlterColumn<Guid>(
                name: "ShipId",
                table: "Order",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);
        }
    }
}