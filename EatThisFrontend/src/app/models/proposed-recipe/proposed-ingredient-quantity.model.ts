import { Unit } from "../unit.model";
import { ProposedIngredient } from "./proposed-ingredient.model";

export class ProposedIngredientQuantity{
    id: number;
    quantity: number;
    description: string;
    proposedIngredient: ProposedIngredient;
    unit: Unit;

    constructor(ingredient: ProposedIngredient = null, description: string = null, quantity: number = null, unit: Unit = null){
        this.quantity = quantity;
        this.description = description;
        this.proposedIngredient = ingredient;
        this.unit = unit;
    }
}