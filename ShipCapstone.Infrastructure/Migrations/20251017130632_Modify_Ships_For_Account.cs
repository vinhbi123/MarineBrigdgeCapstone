using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShipCapstone.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Modify_Ships_For_Account : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Ship_AccountId",
                table: "Ship");

            migrationBuilder.CreateIndex(
                name: "IX_Ship_AccountId",
                table: "Ship",
                column: "AccountId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Ship_AccountId",
                table: "Ship");

            migrationBuilder.CreateIndex(
                name: "IX_Ship_AccountId",
                table: "Ship",
                column: "AccountId",
                unique: true);
        }
    }
}