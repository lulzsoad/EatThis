import { Ingredient } from "../ingredient.model";

export class IngredientPosition{
    public ingredient: Ingredient;
    public isSelected: boolean;
    public isProposed: boolean;
    public isNew: boolean;

    constructor(ingredient: Ingredient, isSelected: boolean = false, isProposed: boolean = false, isNew: boolean = true){
        this.ingredient = ingredient;
        this.isSelected = isSelected;
        this.isProposed = isProposed;
        this.isNew = isNew;
    }
}