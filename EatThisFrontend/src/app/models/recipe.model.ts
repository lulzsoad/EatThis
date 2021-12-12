import { Image } from "./image.model";
import { Category } from "./category.model";
import { IngredientQuantity } from "./ingredient-quantity.model";
import { Step } from "./step.model";

export class Recipe{
    id: number;
    name: string;
    subName: string;
    description: string;
    difficulty: string;
    isVisivble: boolean;
    ingredientQuantities: IngredientQuantity[];
    steps: Step[];
    category: Category;
    image: Image;
    time: number;
    personQuantity: number;
}