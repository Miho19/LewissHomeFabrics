using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lewiss.Pricing.Data.Migrations
{
    /// <inheritdoc />
    public partial class ProductModelUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AboveHeightConstraint",
                table: "Product");

            migrationBuilder.AddColumn<decimal>(
                name: "InstallHeight",
                table: "Product",
                type: "decimal(65,30)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.UpdateData(
                table: "ProductOptionVariation",
                keyColumn: "ProductOptionVariationId",
                keyValue: 41,
                column: "Value",
                value: "Off White");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InstallHeight",
                table: "Product");

            migrationBuilder.AddColumn<bool>(
                name: "AboveHeightConstraint",
                table: "Product",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "ProductOptionVariation",
                keyColumn: "ProductOptionVariationId",
                keyValue: 41,
                column: "Value",
                value: "OffWhite");
        }
    }
}
