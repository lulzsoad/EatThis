using Microsoft.EntityFrameworkCore.Migrations;

namespace EatThisAPI.Migrations
{
    public partial class RecipeCategoryUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "Recipe",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Recipe_CategoryId",
                table: "Recipe",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Recipe_Category_CategoryId",
                table: "Recipe",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Recipe_Category_CategoryId",
                table: "Recipe");

            migrationBuilder.DropIndex(
                name: "IX_Recipe_CategoryId",
                table: "Recipe");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Recipe");
        }
    }
}
