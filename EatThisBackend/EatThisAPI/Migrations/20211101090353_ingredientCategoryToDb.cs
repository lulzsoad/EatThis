using Microsoft.EntityFrameworkCore.Migrations;

namespace EatThisAPI.Migrations
{
    public partial class ingredientCategoryToDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IngredientCategoryId",
                table: "Ingredients",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "IngredientCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IngredientCategories", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Ingredients_IngredientCategoryId",
                table: "Ingredients",
                column: "IngredientCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ingredients_IngredientCategories_IngredientCategoryId",
                table: "Ingredients",
                column: "IngredientCategoryId",
                principalTable: "IngredientCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ingredients_IngredientCategories_IngredientCategoryId",
                table: "Ingredients");

            migrationBuilder.DropTable(
                name: "IngredientCategories");

            migrationBuilder.DropIndex(
                name: "IX_Ingredients_IngredientCategoryId",
                table: "Ingredients");

            migrationBuilder.DropColumn(
                name: "IngredientCategoryId",
                table: "Ingredients");
        }
    }
}
