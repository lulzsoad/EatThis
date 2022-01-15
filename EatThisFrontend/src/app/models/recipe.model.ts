import { Image } from "./image.model";
import { Category } from "./category.model";
import { IngredientQuantity } from "./ingredient-quantity.model";
import { Step } from "./step.model";
import { UserDetails } from "./user-details.model";

export class Recipe{
    id: number;
    name: string;
    subName: string;
    description: string;
    difficulty: string;
    isVisible: boolean;
    ingredientQuantities: IngredientQuantity[];
    steps: Step[];
    category: Category;
    image: string;
    time: number;
    personQuantity: number;
    userDetails: UserDetails
}