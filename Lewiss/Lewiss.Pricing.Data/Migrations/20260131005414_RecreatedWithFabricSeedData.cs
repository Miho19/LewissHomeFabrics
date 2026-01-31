using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Lewiss.Pricing.Data.Migrations
{
    /// <inheritdoc />
    public partial class RecreatedWithFabricSeedData : Migration
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
                    InstallHeight = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
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
                name: "KineticsCellularFabric",
                columns: table => new
                {
                    KineticsCellularFabricId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Colour = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Opacity = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Multiplier = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    ProductOptionVariationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KineticsCellularFabric", x => x.KineticsCellularFabricId);
                    table.ForeignKey(
                        name: "FK_KineticsCellularFabric_ProductOptionVariation_ProductOptionV~",
                        column: x => x.ProductOptionVariationId,
                        principalTable: "ProductOptionVariation",
                        principalColumn: "ProductOptionVariationId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "KineticsRollerFabric",
                columns: table => new
                {
                    KineticsRollerFabricId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Fabric = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Colour = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Opacity = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Multiplier = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    MaxWidth = table.Column<int>(type: "int", nullable: false),
                    MaxHeight = table.Column<int>(type: "int", nullable: false),
                    ProductOptionVariationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KineticsRollerFabric", x => x.KineticsRollerFabricId);
                    table.ForeignKey(
                        name: "FK_KineticsRollerFabric_ProductOptionVariation_ProductOptionVar~",
                        column: x => x.ProductOptionVariationId,
                        principalTable: "ProductOptionVariation",
                        principalColumn: "ProductOptionVariationId",
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
                    { 4, "Fabric" },
                    { 5, "OperationType" },
                    { 6, "OperationSide" },
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
                    { 1, 0.0m, 1, "Inside" },
                    { 2, 0.0m, 1, "Outside" },
                    { 3, 0.0m, 2, "Wood" },
                    { 4, 0.0m, 3, "Kinetics Cellular" },
                    { 5, 0.0m, 3, "Kinetics Roller" },
                    { 6, 250.0m, 5, "Lithium-ion" },
                    { 7, 0.0m, 5, "Cord" },
                    { 8, 0.0m, 5, "Chain" },
                    { 9, 0.00m, 6, "Left" },
                    { 10, 0.00m, 6, "Right" },
                    { 11, 0.00m, 7, "White" },
                    { 12, 0.00m, 7, "Black" },
                    { 13, 0.00m, 8, "White" },
                    { 14, 0.00m, 8, "Black" },
                    { 15, 0.00m, 9, "Front" },
                    { 16, 0.00m, 9, "Back" },
                    { 17, 0.00m, 10, "White" },
                    { 18, 0.00m, 10, "Black" },
                    { 19, 0.00m, 10, "Grey" },
                    { 20, 32.00m, 10, "Stainless" },
                    { 21, 0.00m, 11, "750" },
                    { 22, 0.00m, 11, "1000" },
                    { 23, 0.00m, 11, "1250" },
                    { 24, 0.00m, 11, "1500" },
                    { 25, 0.00m, 11, "1750" },
                    { 26, 0.00m, 11, "2000" },
                    { 27, 0.00m, 11, "2250" },
                    { 28, 0.00m, 11, "2500" },
                    { 29, 0.00m, 11, "2750" },
                    { 30, 0.00m, 12, "Standard" },
                    { 31, 0.00m, 12, "Extra Large" },
                    { 32, 0.00m, 13, "White" },
                    { 33, 0.00m, 13, "Black" },
                    { 34, 0.00m, 14, "Flat" },
                    { 35, 25.00m, 14, "Deluxe" },
                    { 36, 0.00m, 15, "White" },
                    { 37, 0.00m, 15, "Black" },
                    { 38, 0.00m, 15, "Silver" },
                    { 39, 0.00m, 15, "Off White" },
                    { 40, 0.00m, 16, "None" },
                    { 41, 0.00m, 17, "White" },
                    { 42, 0.00m, 17, "Black" },
                    { 43, 0.00m, 4, "Adagio Black LF" },
                    { 44, 0.00m, 4, "Adagio Chilli LF" },
                    { 45, 0.00m, 4, "Adagio Taupe LF" },
                    { 46, 0.00m, 4, "Austro Amaze BO" },
                    { 47, 0.00m, 4, "Austro Anchor BO" },
                    { 48, 0.00m, 4, "Austro Apple BO" },
                    { 49, 0.00m, 4, "Fenescreen 10% Charcoal SS" },
                    { 50, 0.00m, 4, "Fenescreen 10% Coyote SS" },
                    { 51, 0.00m, 4, "Fenescreen 10% Glacier White SS" },
                    { 52, 0.00m, 4, "001 Translucent Cotton" },
                    { 53, 0.00m, 4, "005 Translucent Cream" },
                    { 54, 0.00m, 4, "014 Translucent Water Green" },
                    { 55, 0.00m, 4, "019 Translucent Agate Red" },
                    { 56, 0.00m, 4, "021 Translucent Gray Sheen" },
                    { 57, 0.00m, 4, "023 Translucent Royal Gray" },
                    { 58, 0.00m, 4, "024 Translucent Jean Blue" },
                    { 59, 0.00m, 4, "025 Translucent Black" },
                    { 60, 0.00m, 4, "026 Translucent Federal Blue" },
                    { 61, 0.00m, 4, "030 Translucent Steel Grey" },
                    { 62, 0.00m, 4, "070 Translucent Winter White" },
                    { 63, 0.00m, 4, "102 Translucent Aqua Glass" },
                    { 64, 0.00m, 4, "333 Translucent Warm Chocolate" },
                    { 65, 0.00m, 4, "416 Translucent Taupe A" },
                    { 66, 0.00m, 4, "512 Translucent Flax Green" }
                });

            migrationBuilder.InsertData(
                table: "KineticsCellularFabric",
                columns: new[] { "KineticsCellularFabricId", "Code", "Colour", "Multiplier", "Opacity", "ProductOptionVariationId" },
                values: new object[,]
                {
                    { 1, "001", "Cotton", 1.0m, "Translucent", 52 },
                    { 2, "005", "Cream", 1.0m, "Translucent", 53 },
                    { 3, "014", "Water Green", 1.0m, "Translucent", 54 },
                    { 4, "019", "Agate Red", 1.0m, "Translucent", 55 },
                    { 5, "021", "Gray Sheen", 1.0m, "Translucent", 56 },
                    { 6, "023", "Royal Gray", 1.0m, "Translucent", 57 },
                    { 7, "024", "Jean Blue", 1.0m, "Translucent", 58 },
                    { 8, "025", "Black", 1.0m, "Translucent", 59 },
                    { 9, "026", "Federal Blue", 1.0m, "Translucent", 60 },
                    { 10, "030", "Steel Grey", 1.0m, "Translucent", 61 },
                    { 11, "070", "Winter White", 1.0m, "Translucent", 62 },
                    { 12, "102", "Aqua Glass", 1.0m, "Translucent", 63 },
                    { 13, "333", "Warm Chocolate", 1.0m, "Translucent", 64 },
                    { 14, "416", "Taupe A", 1.0m, "Translucent", 65 },
                    { 15, "512", "Flax Green", 1.0m, "Translucent", 66 }
                });

            migrationBuilder.InsertData(
                table: "KineticsRollerFabric",
                columns: new[] { "KineticsRollerFabricId", "Colour", "Fabric", "MaxHeight", "MaxWidth", "Multiplier", "Opacity", "ProductOptionVariationId" },
                values: new object[,]
                {
                    { 1, "Black", "Adagio", 2010, 3100, 1.25m, "LF", 43 },
                    { 2, "Chilli", "Adagio", 2010, 3100, 1.25m, "LF", 44 },
                    { 3, "Taupe", "Adagio", 2010, 3100, 1.25m, "LF", 45 },
                    { 4, "Amaze", "Austro", 3000, 3000, 0.8m, "BO", 46 },
                    { 5, "Anchor", "Austro", 3000, 3000, 0.8m, "BO", 47 },
                    { 6, "Apple", "Austro", 3000, 3000, 0.8m, "BO", 48 },
                    { 7, "Charcoal", "Fenescreen 10%", 2200, 3000, 0.9m, "SS", 49 },
                    { 8, "Coyote", "Fenescreen 10%", 2200, 3000, 0.9m, "SS", 50 },
                    { 9, "Glacier White", "Fenescreen 10%", 2200, 3000, 0.9m, "SS", 51 }
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
                name: "IX_KineticsCellularFabric_ProductOptionVariationId",
                table: "KineticsCellularFabric",
                column: "ProductOptionVariationId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_KineticsRollerFabric_ProductOptionVariationId",
                table: "KineticsRollerFabric",
                column: "ProductOptionVariationId",
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
                name: "KineticsCellularFabric");

            migrationBuilder.DropTable(
                name: "KineticsRollerFabric");

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
