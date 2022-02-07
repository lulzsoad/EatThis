using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EatThisAPI.Migrations
{
    public partial class ProposedRecipeEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserActivatingCodes_Users_UserId",
                table: "UserActivatingCodes");

            migrationBuilder.DropIndex(
                name: "IX_UserActivatingCodes_UserId",
                table: "UserActivatingCodes");

            migrationBuilder.CreateTable(
                name: "ProposedIngredients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Reference = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IngredientCategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProposedIngredients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProposedIngredients_IngredientCategories_IngredientCategoryId",
                        column: x => x.IngredientCategoryId,
                        principalTable: "IngredientCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProposedRecipes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Difficulty = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Time = table.Column<int>(type: "int", nullable: false),
                    PersonQuantity = table.Column<int>(type: "int", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProposedRecipes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProposedRecipes_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProposedRecipes_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProposedCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProposedRecipeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProposedCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProposedCategories_ProposedRecipes_ProposedRecipeId",
                        column: x => x.ProposedRecipeId,
                        principalTable: "ProposedRecipes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProposedIngredientQuantities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Reference = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UnitId = table.Column<int>(type: "int", nullable: false),
                    ProposedIngredientId = table.Column<int>(type: "int", nullable: false),
                    ProposedRecipeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProposedIngredientQuantities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProposedIngredientQuantities_ProposedIngredients_ProposedIngredientId",
                        column: x => x.ProposedIngredientId,
                        principalTable: "ProposedIngredients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProposedIngredientQuantities_ProposedRecipes_ProposedRecipeId",
                        column: x => x.ProposedRecipeId,
                        principalTable: "ProposedRecipes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProposedIngredientQuantities_Units_UnitId",
                        column: x => x.UnitId,
                        principalTable: "Units",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProposedSteps",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Order = table.Column<int>(type: "int", nullable: false),
                    ProposedRecipeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProposedSteps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProposedSteps_ProposedRecipes_ProposedRecipeId",
                        column: x => x.ProposedRecipeId,
                        principalTable: "ProposedRecipes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProposedCategories_ProposedRecipeId",
                table: "ProposedCategories",
                column: "ProposedRecipeId");

            migrationBuilder.CreateIndex(
                name: "IX_ProposedIngredientQuantities_ProposedIngredientId",
                table: "ProposedIngredientQuantities",
                column: "ProposedIngredientId");

            migrationBuilder.CreateIndex(
                name: "IX_ProposedIngredientQuantities_ProposedRecipeId",
                table: "ProposedIngredientQuantities",
                column: "ProposedRecipeId");

            migrationBuilder.CreateIndex(
                name: "IX_ProposedIngredientQuantities_UnitId",
                table: "ProposedIngredientQuantities",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_ProposedIngredients_IngredientCategoryId",
                table: "ProposedIngredients",
                column: "IngredientCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ProposedRecipes_CategoryId",
                table: "ProposedRecipes",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ProposedRecipes_UserId",
                table: "ProposedRecipes",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ProposedSteps_ProposedRecipeId",
                table: "ProposedSteps",
                column: "ProposedRecipeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProposedCategories");

            migrationBuilder.DropTable(
                name: "ProposedIngredientQuantities");

            migrationBuilder.DropTable(
                name: "ProposedSteps");

            migrationBuilder.DropTable(
                name: "ProposedIngredients");

            migrationBuilder.DropTable(
                name: "ProposedRecipes");

            migrationBuilder.CreateIndex(
                name: "IX_UserActivatingCodes_UserId",
                table: "UserActivatingCodes",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_UserActivatingCodes_Users_UserId",
                table: "UserActivatingCodes",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
