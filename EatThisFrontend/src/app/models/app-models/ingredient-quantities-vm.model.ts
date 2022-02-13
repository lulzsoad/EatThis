import { IngredientQuantity } from "../ingredient-quantity.model";
import { ProposedIngredientQuantity } from "../proposed-recipe/proposed-ingredient-quantity.model";

export class IngredientQuantitiesVM{
    ingredientQuantities: IngredientQuantity[];
    proposedIngredientQuantities: ProposedIngredientQuantity[];
}