import { Unit } from "../unit.model";
import { ProposedIngredient } from "./proposed-ingredient.model";

export class ProposedIngredientQuantity{
    id: number;
    quantity: number;
    description: string;
    proposedIngredient: ProposedIngredient;
    unit: Unit;
}