import { IngredientCategory } from "./ingredient-category.model";

export class Ingredient{
    id: number;
    name: string;
    ingredientCategory: IngredientCategory;

    constructor(name: string = null, ingredientCategory: IngredientCategory = null){
        this.id = null;
        this.name = name;
        this.ingredientCategory = ingredientCategory;
    }
}