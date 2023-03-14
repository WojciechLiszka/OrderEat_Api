using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FastFood.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UserDiet_is_nullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Diets_Dietid",
                table: "Users");

            migrationBuilder.AlterColumn<int>(
                name: "Dietid",
                table: "Users",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Diets_Dietid",
                table: "Users",
                column: "Dietid",
                principalTable: "Diets",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Diets_Dietid",
                table: "Users");

            migrationBuilder.AlterColumn<int>(
                name: "Dietid",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Diets_Dietid",
                table: "Users",
                column: "Dietid",
                principalTable: "Diets",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
