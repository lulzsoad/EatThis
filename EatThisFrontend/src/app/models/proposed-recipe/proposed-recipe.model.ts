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

    constructor(name: string = null, subName: string = null, description: string = null, difficulty: string = null, note: string = null, image: string = null, time: number = null, personQuantity: number = null, ingredientQuantities: IngredientQuantity[] = [], proposedIngredientQuantities: ProposedIngredientQuantity[] = [], steps: Step[] = [], category: Category = null, proposedCategory: ProposedCategory = null, userDetails: UserDetails = null){
        this.name = name;
        this.subName = subName;
        this.description = description;
        this.difficulty = difficulty;
        this.note = note;
        this.image = image;
        this.time = time;
        this.personQuantity = personQuantity;
        this.ingredientQuantities = ingredientQuantities;
        this.proposedIngredientQuantities = proposedIngredientQuantities;
        this.steps = steps;
        this.category = category;
        this.proposedCategory = proposedCategory;
        this.userDetails = userDetails;
    }
}