export class ProposedStep{
    id: number;
    description: string;
    image: string;
    order: number;
    proposedRecipeId: number;

    constructor(description = null, image = null, order = null, proposedRecipeId = null){
        this.description = description;
        this.image = image;
        this.order = order;
        this.proposedRecipeId = proposedRecipeId;
    }
}