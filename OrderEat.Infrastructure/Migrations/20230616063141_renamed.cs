using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderEat.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class renamed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Allergens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Allergens", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Diets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Diets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Fee = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    RestaurantId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Restaurants",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ContactDetails_ContactNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContactDetails_Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContactDetails_Country = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContactDetails_City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContactDetails_Street = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContactDetails_ApartmentNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedById = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Restaurants", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrderedDish",
                columns: table => new
                {
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DishId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderedDish", x => new { x.OrderId, x.Id });
                    table.ForeignKey(
                        name: "FK_OrderedDish_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Dishes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    BasePrize = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BaseCaloricValue = table.Column<int>(type: "int", nullable: false),
                    AllowedCustomization = table.Column<bool>(type: "bit", nullable: false),
                    IsAvilable = table.Column<bool>(type: "bit", nullable: false),
                    RestaurantId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dishes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Dishes_Restaurants_RestaurantId",
                        column: x => x.RestaurantId,
                        principalTable: "Restaurants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateofBirth = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ContactDetails_ContactNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContactDetails_Country = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContactDetails_City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContactDetails_Street = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContactDetails_ApartmentNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RoleId = table.Column<int>(type: "int", nullable: true),
                    DietId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Diets_DietId",
                        column: x => x.DietId,
                        principalTable: "Diets",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Users_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DishSpecialDiet",
                columns: table => new
                {
                    AllowedForDietsId = table.Column<int>(type: "int", nullable: false),
                    DishesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DishSpecialDiet", x => new { x.AllowedForDietsId, x.DishesId });
                    table.ForeignKey(
                        name: "FK_DishSpecialDiet_Diets_AllowedForDietsId",
                        column: x => x.AllowedForDietsId,
                        principalTable: "Diets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DishSpecialDiet_Dishes_DishesId",
                        column: x => x.DishesId,
                        principalTable: "Dishes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Ingredients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Prize = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsRequired = table.Column<bool>(type: "bit", nullable: false),
                    DishId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ingredients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ingredients_Dishes_DishId",
                        column: x => x.DishId,
                        principalTable: "Dishes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AllergenIngredient",
                columns: table => new
                {
                    AllergensId = table.Column<int>(type: "int", nullable: false),
                    IngredientsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AllergenIngredient", x => new { x.AllergensId, x.IngredientsId });
                    table.ForeignKey(
                        name: "FK_AllergenIngredient_Allergens_AllergensId",
                        column: x => x.AllergensId,
                        principalTable: "Allergens",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AllergenIngredient_Ingredients_IngredientsId",
                        column: x => x.IngredientsId,
                        principalTable: "Ingredients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AllergenIngredient_IngredientsId",
                table: "AllergenIngredient",
                column: "IngredientsId");

            migrationBuilder.CreateIndex(
                name: "IX_Dishes_RestaurantId",
                table: "Dishes",
                column: "RestaurantId");

            migrationBuilder.CreateIndex(
                name: "IX_DishSpecialDiet_DishesId",
                table: "DishSpecialDiet",
                column: "DishesId");

            migrationBuilder.CreateIndex(
                name: "IX_Ingredients_DishId",
                table: "Ingredients",
                column: "DishId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_DietId",
                table: "Users",
                column: "DietId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                table: "Users",
                column: "RoleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AllergenIngredient");

            migrationBuilder.DropTable(
                name: "DishSpecialDiet");

            migrationBuilder.DropTable(
                name: "OrderedDish");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Allergens");

            migrationBuilder.DropTable(
                name: "Ingredients");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Diets");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Dishes");

            migrationBuilder.DropTable(
                name: "Restaurants");
        }
    }
}
