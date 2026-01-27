using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Lewiss.Pricing.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Customer",
                columns: table => new
                {
                    CustomerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ExternalMapping = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    FamilyName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Street = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    City = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Suburb = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Mobile = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.CustomerId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ProductOption",
                columns: table => new
                {
                    ProductOptionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductOption", x => x.ProductOptionId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Worksheet",
                columns: table => new
                {
                    WorksheetId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ExternalMapping = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    Discount = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    NewBuild = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CallOutFee = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    CustomerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Worksheet", x => x.WorksheetId);
                    table.ForeignKey(
                        name: "FK_Worksheet_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ProductOptionVariation",
                columns: table => new
                {
                    ProductOptionVariationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Price = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    ProductOptionId = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductOptionVariation", x => x.ProductOptionVariationId);
                    table.ForeignKey(
                        name: "FK_ProductOptionVariation_ProductOption_ProductOptionId",
                        column: x => x.ProductOptionId,
                        principalTable: "ProductOption",
                        principalColumn: "ProductOptionId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ExternalMapping = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Price = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    Location = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Width = table.Column<int>(type: "int", nullable: false),
                    Height = table.Column<int>(type: "int", nullable: false),
                    Reveal = table.Column<int>(type: "int", nullable: false),
                    AboveHeightConstraint = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    RemoteNumber = table.Column<int>(type: "int", nullable: false),
                    RemoteChannel = table.Column<int>(type: "int", nullable: false),
                    WorksheetId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.ProductId);
                    table.ForeignKey(
                        name: "FK_Product_Worksheet_WorksheetId",
                        column: x => x.WorksheetId,
                        principalTable: "Worksheet",
                        principalColumn: "WorksheetId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ProductProductOptionVariation",
                columns: table => new
                {
                    OptionVariationsProductOptionVariationId = table.Column<int>(type: "int", nullable: false),
                    ProductsProductId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductProductOptionVariation", x => new { x.OptionVariationsProductOptionVariationId, x.ProductsProductId });
                    table.ForeignKey(
                        name: "FK_ProductProductOptionVariation_ProductOptionVariation_OptionV~",
                        column: x => x.OptionVariationsProductOptionVariationId,
                        principalTable: "ProductOptionVariation",
                        principalColumn: "ProductOptionVariationId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductProductOptionVariation_Product_ProductsProductId",
                        column: x => x.ProductsProductId,
                        principalTable: "Product",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "ProductOption",
                columns: new[] { "ProductOptionId", "Name" },
                values: new object[,]
                {
                    { 1, "FitType" },
                    { 2, "FixingTo" },
                    { 3, "ProductType" },
                    { 4, "Fabric" }
                });

            migrationBuilder.InsertData(
                table: "ProductOptionVariation",
                columns: new[] { "ProductOptionVariationId", "Price", "ProductOptionId", "Value" },
                values: new object[,]
                {
                    { 1, 0.0m, 1, "Inside" },
                    { 2, 0.0m, 1, "Outside" },
                    { 3, 0.0m, 2, "Wood" },
                    { 4, 0.0m, 3, "Kinetics Cellular" },
                    { 5, 0.0m, 3, "Kinetics Roller" },
                    { 6, 0.0m, 4, "Translucent White" },
                    { 7, 0.0m, 4, "Everyday Vinyl Collection - Polar" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Customer_Email",
                table: "Customer",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Customer_Mobile",
                table: "Customer",
                column: "Mobile",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Product_WorksheetId",
                table: "Product",
                column: "WorksheetId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductOptionVariation_ProductOptionId",
                table: "ProductOptionVariation",
                column: "ProductOptionId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductProductOptionVariation_ProductsProductId",
                table: "ProductProductOptionVariation",
                column: "ProductsProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Worksheet_CustomerId",
                table: "Worksheet",
                column: "CustomerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductProductOptionVariation");

            migrationBuilder.DropTable(
                name: "ProductOptionVariation");

            migrationBuilder.DropTable(
                name: "Product");

            migrationBuilder.DropTable(
                name: "ProductOption");

            migrationBuilder.DropTable(
                name: "Worksheet");

            migrationBuilder.DropTable(
                name: "Customer");
        }
    }
}
