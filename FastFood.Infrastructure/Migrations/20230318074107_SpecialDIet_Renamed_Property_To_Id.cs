using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FastFood.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SpecialDIet_Renamed_Property_To_Id : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DishSpecialDiet_Diets_AllowedForDietsid",
                table: "DishSpecialDiet");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Diets_Dietid",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "Dietid",
                table: "Users",
                newName: "DietId");

            migrationBuilder.RenameIndex(
                name: "IX_Users_Dietid",
                table: "Users",
                newName: "IX_Users_DietId");

            migrationBuilder.RenameColumn(
                name: "AllowedForDietsid",
                table: "DishSpecialDiet",
                newName: "AllowedForDietsId");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Diets",
                newName: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DishSpecialDiet_Diets_AllowedForDietsId",
                table: "DishSpecialDiet",
                column: "AllowedForDietsId",
                principalTable: "Diets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Diets_DietId",
                table: "Users",
                column: "DietId",
                principalTable: "Diets",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DishSpecialDiet_Diets_AllowedForDietsId",
                table: "DishSpecialDiet");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Diets_DietId",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "DietId",
                table: "Users",
                newName: "Dietid");

            migrationBuilder.RenameIndex(
                name: "IX_Users_DietId",
                table: "Users",
                newName: "IX_Users_Dietid");

            migrationBuilder.RenameColumn(
                name: "AllowedForDietsId",
                table: "DishSpecialDiet",
                newName: "AllowedForDietsid");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Diets",
                newName: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_DishSpecialDiet_Diets_AllowedForDietsid",
                table: "DishSpecialDiet",
                column: "AllowedForDietsid",
                principalTable: "Diets",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Diets_Dietid",
                table: "Users",
                column: "Dietid",
                principalTable: "Diets",
                principalColumn: "id");
        }
    }
}
