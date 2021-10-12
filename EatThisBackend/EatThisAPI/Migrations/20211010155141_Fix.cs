using Microsoft.EntityFrameworkCore.Migrations;

namespace EatThisAPI.Migrations
{
    public partial class Fix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IngredientQuantity_Ingredient_IngredientId",
                table: "IngredientQuantity");

            migrationBuilder.DropForeignKey(
                name: "FK_IngredientQuantity_Recipe_RecipeId",
                table: "IngredientQuantity");

            migrationBuilder.DropForeignKey(
                name: "FK_IngredientQuantity_Unit_UnitId",
                table: "IngredientQuantity");

            migrationBuilder.DropForeignKey(
                name: "FK_Recipe_Category_CategoryId",
                table: "Recipe");

            migrationBuilder.DropForeignKey(
                name: "FK_RecipeImage_Recipe_Id",
                table: "RecipeImage");

            migrationBuilder.DropForeignKey(
                name: "FK_Step_Recipe_RecipeId",
                table: "Step");

            migrationBuilder.DropForeignKey(
                name: "FK_StepImage_Step_Id",
                table: "StepImage");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Unit",
                table: "Unit");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StepImage",
                table: "StepImage");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Step",
                table: "Step");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Recipe",
                table: "Recipe");

            migrationBuilder.DropPrimaryKey(
                name: "PK_IngredientQuantity",
                table: "IngredientQuantity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Ingredient",
                table: "Ingredient");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Category",
                table: "Category");

            migrationBuilder.RenameTable(
                name: "Unit",
                newName: "Units");

            migrationBuilder.RenameTable(
                name: "StepImage",
                newName: "Images");

            migrationBuilder.RenameTable(
                name: "Step",
                newName: "Steps");

            migrationBuilder.RenameTable(
                name: "Recipe",
                newName: "Recipes");

            migrationBuilder.RenameTable(
                name: "IngredientQuantity",
                newName: "IngredientQuantities");

            migrationBuilder.RenameTable(
                name: "Ingredient",
                newName: "Ingredients");

            migrationBuilder.RenameTable(
                name: "Category",
                newName: "Categories");

            migrationBuilder.RenameIndex(
                name: "IX_Step_RecipeId",
                table: "Steps",
                newName: "IX_Steps_RecipeId");

            migrationBuilder.RenameIndex(
                name: "IX_Recipe_CategoryId",
                table: "Recipes",
                newName: "IX_Recipes_CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_IngredientQuantity_UnitId",
                table: "IngredientQuantities",
                newName: "IX_IngredientQuantities_UnitId");

            migrationBuilder.RenameIndex(
                name: "IX_IngredientQuantity_RecipeId",
                table: "IngredientQuantities",
                newName: "IX_IngredientQuantities_RecipeId");

            migrationBuilder.RenameIndex(
                name: "IX_IngredientQuantity_IngredientId",
                table: "IngredientQuantities",
                newName: "IX_IngredientQuantities_IngredientId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Units",
                table: "Units",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Images",
                table: "Images",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Steps",
                table: "Steps",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Recipes",
                table: "Recipes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_IngredientQuantities",
                table: "IngredientQuantities",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Ingredients",
                table: "Ingredients",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Categories",
                table: "Categories",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Images_Steps_Id",
                table: "Images",
                column: "Id",
                principalTable: "Steps",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_IngredientQuantities_Ingredients_IngredientId",
                table: "IngredientQuantities",
                column: "IngredientId",
                principalTable: "Ingredients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_IngredientQuantities_Recipes_RecipeId",
                table: "IngredientQuantities",
                column: "RecipeId",
                principalTable: "Recipes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_IngredientQuantities_Units_UnitId",
                table: "IngredientQuantities",
                column: "UnitId",
                principalTable: "Units",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeImage_Recipes_Id",
                table: "RecipeImage",
                column: "Id",
                principalTable: "Recipes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Recipes_Categories_CategoryId",
                table: "Recipes",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Steps_Recipes_RecipeId",
                table: "Steps",
                column: "RecipeId",
                principalTable: "Recipes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_Steps_Id",
                table: "Images");

            migrationBuilder.DropForeignKey(
                name: "FK_IngredientQuantities_Ingredients_IngredientId",
                table: "IngredientQuantities");

            migrationBuilder.DropForeignKey(
                name: "FK_IngredientQuantities_Recipes_RecipeId",
                table: "IngredientQuantities");

            migrationBuilder.DropForeignKey(
                name: "FK_IngredientQuantities_Units_UnitId",
                table: "IngredientQuantities");

            migrationBuilder.DropForeignKey(
                name: "FK_RecipeImage_Recipes_Id",
                table: "RecipeImage");

            migrationBuilder.DropForeignKey(
                name: "FK_Recipes_Categories_CategoryId",
                table: "Recipes");

            migrationBuilder.DropForeignKey(
                name: "FK_Steps_Recipes_RecipeId",
                table: "Steps");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Units",
                table: "Units");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Steps",
                table: "Steps");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Recipes",
                table: "Recipes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Ingredients",
                table: "Ingredients");

            migrationBuilder.DropPrimaryKey(
                name: "PK_IngredientQuantities",
                table: "IngredientQuantities");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Images",
                table: "Images");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Categories",
                table: "Categories");

            migrationBuilder.RenameTable(
                name: "Units",
                newName: "Unit");

            migrationBuilder.RenameTable(
                name: "Steps",
                newName: "Step");

            migrationBuilder.RenameTable(
                name: "Recipes",
                newName: "Recipe");

            migrationBuilder.RenameTable(
                name: "Ingredients",
                newName: "Ingredient");

            migrationBuilder.RenameTable(
                name: "IngredientQuantities",
                newName: "IngredientQuantity");

            migrationBuilder.RenameTable(
                name: "Images",
                newName: "StepImage");

            migrationBuilder.RenameTable(
                name: "Categories",
                newName: "Category");

            migrationBuilder.RenameIndex(
                name: "IX_Steps_RecipeId",
                table: "Step",
                newName: "IX_Step_RecipeId");

            migrationBuilder.RenameIndex(
                name: "IX_Recipes_CategoryId",
                table: "Recipe",
                newName: "IX_Recipe_CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_IngredientQuantities_UnitId",
                table: "IngredientQuantity",
                newName: "IX_IngredientQuantity_UnitId");

            migrationBuilder.RenameIndex(
                name: "IX_IngredientQuantities_RecipeId",
                table: "IngredientQuantity",
                newName: "IX_IngredientQuantity_RecipeId");

            migrationBuilder.RenameIndex(
                name: "IX_IngredientQuantities_IngredientId",
                table: "IngredientQuantity",
                newName: "IX_IngredientQuantity_IngredientId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Unit",
                table: "Unit",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Step",
                table: "Step",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Recipe",
                table: "Recipe",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Ingredient",
                table: "Ingredient",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_IngredientQuantity",
                table: "IngredientQuantity",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StepImage",
                table: "StepImage",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Category",
                table: "Category",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_IngredientQuantity_Ingredient_IngredientId",
                table: "IngredientQuantity",
                column: "IngredientId",
                principalTable: "Ingredient",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_IngredientQuantity_Recipe_RecipeId",
                table: "IngredientQuantity",
                column: "RecipeId",
                principalTable: "Recipe",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_IngredientQuantity_Unit_UnitId",
                table: "IngredientQuantity",
                column: "UnitId",
                principalTable: "Unit",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Recipe_Category_CategoryId",
                table: "Recipe",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeImage_Recipe_Id",
                table: "RecipeImage",
                column: "Id",
                principalTable: "Recipe",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Step_Recipe_RecipeId",
                table: "Step",
                column: "RecipeId",
                principalTable: "Recipe",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StepImage_Step_Id",
                table: "StepImage",
                column: "Id",
                principalTable: "Step",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
