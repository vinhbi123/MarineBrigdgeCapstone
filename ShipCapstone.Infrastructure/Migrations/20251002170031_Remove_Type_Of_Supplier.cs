using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShipCapstone.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Remove_Type_Of_Supplier : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Supplier");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Supplier",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
