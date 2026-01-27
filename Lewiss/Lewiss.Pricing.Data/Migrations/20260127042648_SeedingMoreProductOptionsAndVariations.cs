using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Lewiss.Pricing.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedingMoreProductOptionsAndVariations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ProductOption",
                columns: new[] { "ProductOptionId", "Name" },
                values: new object[,]
                {
                    { 5, "OperationType" },
                    { 6, "OpertionSide" },
                    { 7, "HeadrailColour" },
                    { 8, "SideChannelColour" },
                    { 9, "RollType" },
                    { 10, "ChainColour" },
                    { 11, "ChainLength" },
                    { 12, "BracketType" },
                    { 13, "BracketColour" },
                    { 14, "BottomRailType" },
                    { 15, "BottomRailColour" },
                    { 16, "PelmetType" },
                    { 17, "PelmetColour" }
                });

            migrationBuilder.InsertData(
                table: "ProductOptionVariation",
                columns: new[] { "ProductOptionVariationId", "Price", "ProductOptionId", "Value" },
                values: new object[,]
                {
                    { 8, 250.0m, 5, "Lithium-ion" },
                    { 9, 0.0m, 5, "Cord" },
                    { 10, 0.0m, 5, "Chain" },
                    { 11, 0.00m, 6, "Left" },
                    { 12, 0.00m, 6, "Right" },
                    { 13, 0.00m, 7, "White" },
                    { 14, 0.00m, 7, "Black" },
                    { 15, 0.00m, 8, "White" },
                    { 16, 0.00m, 8, "Black" },
                    { 17, 0.00m, 9, "Front" },
                    { 18, 0.00m, 9, "Back" },
                    { 19, 0.00m, 10, "White" },
                    { 20, 0.00m, 10, "Black" },
                    { 21, 0.00m, 10, "Grey" },
                    { 22, 32.00m, 10, "Stainless" },
                    { 23, 0.00m, 11, "750" },
                    { 24, 0.00m, 11, "1000" },
                    { 25, 0.00m, 11, "1250" },
                    { 26, 0.00m, 11, "1500" },
                    { 27, 0.00m, 11, "1750" },
                    { 28, 0.00m, 11, "2000" },
                    { 29, 0.00m, 11, "2250" },
                    { 30, 0.00m, 11, "2500" },
                    { 31, 0.00m, 11, "2750" },
                    { 32, 0.00m, 12, "Standard" },
                    { 33, 0.00m, 12, "Extra Large" },
                    { 34, 0.00m, 13, "White" },
                    { 35, 0.00m, 13, "Black" },
                    { 36, 0.00m, 14, "Flat" },
                    { 37, 25.00m, 14, "Deluxe" },
                    { 38, 0.00m, 15, "White" },
                    { 39, 0.00m, 15, "Black" },
                    { 40, 0.00m, 15, "Silver" },
                    { 41, 0.00m, 15, "OffWhite" },
                    { 42, 0.00m, 16, "None" },
                    { 43, 0.00m, 17, "White" },
                    { 44, 0.00m, 17, "Black" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ProductOptionVariation",
                keyColumn: "ProductOptionVariationId",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "ProductOptionVariation",
                keyColumn: "ProductOptionVariationId",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "ProductOptionVariation",
                keyColumn: "ProductOptionVariationId",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "ProductOptionVariation",
                keyColumn: "ProductOptionVariationId",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "ProductOptionVariation",
                keyColumn: "ProductOptionVariationId",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "ProductOptionVariation",
                keyColumn: "ProductOptionVariationId",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "ProductOptionVariation",
                keyColumn: "ProductOptionVariationId",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "ProductOptionVariation",
                keyColumn: "ProductOptionVariationId",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "ProductOptionVariation",
                keyColumn: "ProductOptionVariationId",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "ProductOptionVariation",
                keyColumn: "ProductOptionVariationId",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "ProductOptionVariation",
                keyColumn: "ProductOptionVariationId",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "ProductOptionVariation",
                keyColumn: "ProductOptionVariationId",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "ProductOptionVariation",
                keyColumn: "ProductOptionVariationId",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "ProductOptionVariation",
                keyColumn: "ProductOptionVariationId",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "ProductOptionVariation",
                keyColumn: "ProductOptionVariationId",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "ProductOptionVariation",
                keyColumn: "ProductOptionVariationId",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "ProductOptionVariation",
                keyColumn: "ProductOptionVariationId",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "ProductOptionVariation",
                keyColumn: "ProductOptionVariationId",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "ProductOptionVariation",
                keyColumn: "ProductOptionVariationId",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "ProductOptionVariation",
                keyColumn: "ProductOptionVariationId",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "ProductOptionVariation",
                keyColumn: "ProductOptionVariationId",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "ProductOptionVariation",
                keyColumn: "ProductOptionVariationId",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "ProductOptionVariation",
                keyColumn: "ProductOptionVariationId",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "ProductOptionVariation",
                keyColumn: "ProductOptionVariationId",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "ProductOptionVariation",
                keyColumn: "ProductOptionVariationId",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "ProductOptionVariation",
                keyColumn: "ProductOptionVariationId",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "ProductOptionVariation",
                keyColumn: "ProductOptionVariationId",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "ProductOptionVariation",
                keyColumn: "ProductOptionVariationId",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "ProductOptionVariation",
                keyColumn: "ProductOptionVariationId",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "ProductOptionVariation",
                keyColumn: "ProductOptionVariationId",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "ProductOptionVariation",
                keyColumn: "ProductOptionVariationId",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "ProductOptionVariation",
                keyColumn: "ProductOptionVariationId",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "ProductOptionVariation",
                keyColumn: "ProductOptionVariationId",
                keyValue: 40);

            migrationBuilder.DeleteData(
                table: "ProductOptionVariation",
                keyColumn: "ProductOptionVariationId",
                keyValue: 41);

            migrationBuilder.DeleteData(
                table: "ProductOptionVariation",
                keyColumn: "ProductOptionVariationId",
                keyValue: 42);

            migrationBuilder.DeleteData(
                table: "ProductOptionVariation",
                keyColumn: "ProductOptionVariationId",
                keyValue: 43);

            migrationBuilder.DeleteData(
                table: "ProductOptionVariation",
                keyColumn: "ProductOptionVariationId",
                keyValue: 44);

            migrationBuilder.DeleteData(
                table: "ProductOption",
                keyColumn: "ProductOptionId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "ProductOption",
                keyColumn: "ProductOptionId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "ProductOption",
                keyColumn: "ProductOptionId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "ProductOption",
                keyColumn: "ProductOptionId",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "ProductOption",
                keyColumn: "ProductOptionId",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "ProductOption",
                keyColumn: "ProductOptionId",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "ProductOption",
                keyColumn: "ProductOptionId",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "ProductOption",
                keyColumn: "ProductOptionId",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "ProductOption",
                keyColumn: "ProductOptionId",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "ProductOption",
                keyColumn: "ProductOptionId",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "ProductOption",
                keyColumn: "ProductOptionId",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "ProductOption",
                keyColumn: "ProductOptionId",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "ProductOption",
                keyColumn: "ProductOptionId",
                keyValue: 17);
        }
    }
}
