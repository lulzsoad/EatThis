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

    constructor(ingredient: Ingredient = null, quantity: number = null, description: string = null, unit: Unit = null){
        this.ingredient = ingredient;
        this.ingredientId = ingredient?.id;
        this.quantity = quantity;
        this.description = description;
        this.unit = unit;
        this.unitId = unit?.id;
    }
}