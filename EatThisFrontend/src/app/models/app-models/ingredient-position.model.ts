import { Ingredient } from "../ingredient.model";

export class IngredientPosition{
    public ingredient: Ingredient;
    public isSelected: boolean;

    constructor(ingredient: Ingredient, isSelected: boolean = false){
        this.ingredient = ingredient;
        this.isSelected = isSelected;
    }
}