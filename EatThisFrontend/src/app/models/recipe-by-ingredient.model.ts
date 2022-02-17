import { IngredientQuantity } from "./ingredient-quantity.model";
import { Recipe } from "./recipe.model";

export class RecipeByIngredient{
    recipe: Recipe;
    includedIngredients: IngredientQuantity[] = [];
    missingIngredients: IngredientQuantity[] = [];
    includedIngredientsCount: number;
    missingIngredientsCount: number;
}