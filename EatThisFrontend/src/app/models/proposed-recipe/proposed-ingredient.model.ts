import { IngredientCategory } from "../ingredient-category.model";

export class ProposedIngredient{
    id: number;
    name: string;
    ingredientCategory: IngredientCategory

    constructor(name: string = null, ingredientCategory = null){
        this.name = name;
        this.ingredientCategory = ingredientCategory;
    }
}