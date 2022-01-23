export class Step{
    id: number;
    description: string;
    image: string;
    order: number;
    recipeId: number;

    constructor(description = null, image = null, order = null, recipeId = null){
        this.description = description;
        this.image = image;
        this.order = order;
        this.recipeId = recipeId;
    }
}