import { Ingredient } from "./ingredient.model";
import { Unit } from "./unit.model";

export class IngredientQuantity{
    id: number;
    quantity: number;
    description: string;
    unitId: number;
    unit: Unit;
    ingredientId: number;
    ingredient: Ingredient;
    recipeId: number;
}