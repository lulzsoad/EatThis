using Microsoft.EntityFrameworkCore.Migrations;

namespace EatThisAPI.Migrations
{
    public partial class ProposedIngredientQuantityUpdate2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProposedIngredientQuantities_ProposedIngredients_ProposedIngredientId",
                table: "ProposedIngredientQuantities");

            migrationBuilder.AlterColumn<int>(
                name: "ProposedIngredientId",
                table: "ProposedIngredientQuantities",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_ProposedIngredientQuantities_ProposedIngredients_ProposedIngredientId",
                table: "ProposedIngredientQuantities",
                column: "ProposedIngredientId",
                principalTable: "ProposedIngredients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProposedIngredientQuantities_ProposedIngredients_ProposedIngredientId",
                table: "ProposedIngredientQuantities");

            migrationBuilder.AlterColumn<int>(
                name: "ProposedIngredientId",
                table: "ProposedIngredientQuantities",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ProposedIngredientQuantities_ProposedIngredients_ProposedIngredientId",
                table: "ProposedIngredientQuantities",
                column: "ProposedIngredientId",
                principalTable: "ProposedIngredients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
