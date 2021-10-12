using Microsoft.EntityFrameworkCore.Migrations;

namespace EatThisAPI.Migrations
{
    public partial class Correction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Recipe_Ingredient_IngredientId",
                table: "Recipe");

            migrationBuilder.DropIndex(
                name: "IX_Recipe_IngredientId",
                table: "Recipe");

            migrationBuilder.DropColumn(
                name: "IngredientId",
                table: "Recipe");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IngredientId",
                table: "Recipe",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Recipe_IngredientId",
                table: "Recipe",
                column: "IngredientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Recipe_Ingredient_IngredientId",
                table: "Recipe",
                column: "IngredientId",
                principalTable: "Ingredient",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
