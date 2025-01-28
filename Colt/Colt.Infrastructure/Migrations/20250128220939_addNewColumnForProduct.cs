using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Colt.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addNewColumnForProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MeasurementType",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "Weight");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MeasurementType",
                table: "Products");
        }
    }
}
