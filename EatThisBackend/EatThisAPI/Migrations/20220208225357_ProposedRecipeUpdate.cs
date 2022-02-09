using Microsoft.EntityFrameworkCore.Migrations;

namespace EatThisAPI.Migrations
{
    public partial class ProposedRecipeUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProposedCategories_ProposedRecipes_ProposedRecipeId",
                table: "ProposedCategories");

            migrationBuilder.DropIndex(
                name: "IX_ProposedCategories_ProposedRecipeId",
                table: "ProposedCategories");

            migrationBuilder.DropColumn(
                name: "ProposedRecipeId",
                table: "ProposedCategories");

            migrationBuilder.AddColumn<int>(
                name: "ProposedCategoryId",
                table: "ProposedRecipes",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProposedRecipes_ProposedCategoryId",
                table: "ProposedRecipes",
                column: "ProposedCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProposedRecipes_ProposedCategories_ProposedCategoryId",
                table: "ProposedRecipes",
                column: "ProposedCategoryId",
                principalTable: "ProposedCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProposedRecipes_ProposedCategories_ProposedCategoryId",
                table: "ProposedRecipes");

            migrationBuilder.DropIndex(
                name: "IX_ProposedRecipes_ProposedCategoryId",
                table: "ProposedRecipes");

            migrationBuilder.DropColumn(
                name: "ProposedCategoryId",
                table: "ProposedRecipes");

            migrationBuilder.AddColumn<int>(
                name: "ProposedRecipeId",
                table: "ProposedCategories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ProposedCategories_ProposedRecipeId",
                table: "ProposedCategories",
                column: "ProposedRecipeId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProposedCategories_ProposedRecipes_ProposedRecipeId",
                table: "ProposedCategories",
                column: "ProposedRecipeId",
                principalTable: "ProposedRecipes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
