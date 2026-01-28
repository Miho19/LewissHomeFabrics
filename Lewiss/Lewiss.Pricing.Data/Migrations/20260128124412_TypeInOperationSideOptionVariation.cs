using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lewiss.Pricing.Data.Migrations
{
    /// <inheritdoc />
    public partial class TypeInOperationSideOptionVariation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ProductOption",
                keyColumn: "ProductOptionId",
                keyValue: 6,
                column: "Name",
                value: "OperationSide");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ProductOption",
                keyColumn: "ProductOptionId",
                keyValue: 6,
                column: "Name",
                value: "OpertionSide");
        }
    }
}
