using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShipCapstone.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Add_Commission_Fee_For_Supplier_And_Boatyard : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "CommissionFeePercent",
                table: "Supplier",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 5m);

            migrationBuilder.AddColumn<decimal>(
                name: "CommissionFeePercent",
                table: "Boatyard",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 5m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CommissionFeePercent",
                table: "Supplier");

            migrationBuilder.DropColumn(
                name: "CommissionFeePercent",
                table: "Boatyard");
        }
    }
}
