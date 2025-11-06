using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShipCapstone.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Add_Captain_For_Ship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Booking_Account_AccountId",
                table: "Booking");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_Account_AccountId",
                table: "Order");

            migrationBuilder.DropTable(
                name: "Delivery");

            migrationBuilder.DropTable(
                name: "Payment");

            migrationBuilder.RenameColumn(
                name: "Currency",
                table: "Transaction",
                newName: "TransactionCode");

            migrationBuilder.AlterColumn<Guid>(
                name: "OrderId",
                table: "Transaction",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "BookingId",
                table: "Transaction",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CaptainId",
                table: "Ship",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "AccountId",
                table: "Order",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "ShipId",
                table: "Order",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<Guid>(
                name: "AccountId",
                table: "Booking",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "ShipId",
                table: "Booking",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_BookingId",
                table: "Transaction",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_Ship_CaptainId",
                table: "Ship",
                column: "CaptainId",
                unique: true,
                filter: "[CaptainId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Order_ShipId",
                table: "Order",
                column: "ShipId");

            migrationBuilder.CreateIndex(
                name: "IX_Booking_ShipId",
                table: "Booking",
                column: "ShipId");

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_Account_AccountId",
                table: "Booking",
                column: "AccountId",
                principalTable: "Account",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_Ship_ShipId",
                table: "Booking",
                column: "ShipId",
                principalTable: "Ship",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Account_AccountId",
                table: "Order",
                column: "AccountId",
                principalTable: "Account",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Ship_ShipId",
                table: "Order",
                column: "ShipId",
                principalTable: "Ship",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Ship_Account_CaptainId",
                table: "Ship",
                column: "CaptainId",
                principalTable: "Account",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_Booking_BookingId",
                table: "Transaction",
                column: "BookingId",
                principalTable: "Booking",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Booking_Account_AccountId",
                table: "Booking");

            migrationBuilder.DropForeignKey(
                name: "FK_Booking_Ship_ShipId",
                table: "Booking");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_Account_AccountId",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_Ship_ShipId",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_Ship_Account_CaptainId",
                table: "Ship");

            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_Booking_BookingId",
                table: "Transaction");

            migrationBuilder.DropIndex(
                name: "IX_Transaction_BookingId",
                table: "Transaction");

            migrationBuilder.DropIndex(
                name: "IX_Ship_CaptainId",
                table: "Ship");

            migrationBuilder.DropIndex(
                name: "IX_Order_ShipId",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Booking_ShipId",
                table: "Booking");

            migrationBuilder.DropColumn(
                name: "BookingId",
                table: "Transaction");

            migrationBuilder.DropColumn(
                name: "CaptainId",
                table: "Ship");

            migrationBuilder.DropColumn(
                name: "ShipId",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "ShipId",
                table: "Booking");

            migrationBuilder.RenameColumn(
                name: "TransactionCode",
                table: "Transaction",
                newName: "Currency");

            migrationBuilder.AlterColumn<Guid>(
                name: "OrderId",
                table: "Transaction",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "AccountId",
                table: "Order",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "AccountId",
                table: "Booking",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Delivery",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ShipId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ArrivalTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DepartureTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Delivery", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Delivery_Order_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Order",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Delivery_Ship_ShipId",
                        column: x => x.ShipId,
                        principalTable: "Ship",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Payment",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PaymentMethod = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Payment_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Delivery_OrderId",
                table: "Delivery",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Delivery_ShipId",
                table: "Delivery",
                column: "ShipId");

            migrationBuilder.CreateIndex(
                name: "IX_Payment_AccountId",
                table: "Payment",
                column: "AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_Account_AccountId",
                table: "Booking",
                column: "AccountId",
                principalTable: "Account",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Account_AccountId",
                table: "Order",
                column: "AccountId",
                principalTable: "Account",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
