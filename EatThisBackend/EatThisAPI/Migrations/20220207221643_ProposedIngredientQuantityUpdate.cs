using Microsoft.EntityFrameworkCore.Migrations;

namespace EatThisAPI.Migrations
{
    public partial class ProposedIngredientQuantityUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IngredientId",
                table: "ProposedIngredientQuantities",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProposedIngredientQuantities_IngredientId",
                table: "ProposedIngredientQuantities",
                column: "IngredientId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProposedIngredientQuantities_Ingredients_IngredientId",
                table: "ProposedIngredientQuantities",
                column: "IngredientId",
                principalTable: "Ingredients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProposedIngredientQuantities_Ingredients_IngredientId",
                table: "ProposedIngredientQuantities");

            migrationBuilder.DropIndex(
                name: "IX_ProposedIngredientQuantities_IngredientId",
                table: "ProposedIngredientQuantities");

            migrationBuilder.DropColumn(
                name: "IngredientId",
                table: "ProposedIngredientQuantities");
        }
    }
}
