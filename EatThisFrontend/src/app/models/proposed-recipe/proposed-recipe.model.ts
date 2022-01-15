import { Category } from "../category.model";
import { IngredientQuantity } from "../ingredient-quantity.model";
import { Step } from "../step.model";
import { UserDetails } from "../user-details.model";
import { ProposedCategory } from "./proposed-category.model";
import { ProposedIngredientQuantity } from "./proposed-ingredient-quantity.model";

export class ProposedRecipe{
    id: number;
    name: string;
    subName: string;
    description: string;
    difficulty: string;
    note: string;
    image: string;
    time: number;
    personQuantity: number;
    ingredientQuantities: IngredientQuantity[];
    proposedIngredientQuantities: ProposedIngredientQuantity[];
    steps: Step[];
    category: Category;
    proposedCategory: ProposedCategory;
    userDetails: UserDetails;
}