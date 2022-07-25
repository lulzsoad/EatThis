import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { AppConfig } from "../app-config/app.config";
import { DataChunk } from "../models/app-models/data-chunk.model";
import { DiscardRecipeRequest } from "../models/app-models/discard-recipe-request.model";
import { ProposedCategory } from "../models/proposed-recipe/proposed-category.model";
import { ProposedRecipe } from "../models/proposed-recipe/proposed-recipe.model";
import { RecipeByIngredient } from "../models/recipe-by-ingredient.model";
import { Recipe } from "../models/recipe.model";

@Injectable()
export class RecipeService{
    private apiUrl = `${AppConfig.APP_URL}api/recipe`;

    constructor(private httpClient: HttpClient)
    { }

    addRecipe(recipe: Recipe){
        return this.httpClient.post<number>(this.apiUrl, recipe);
    }

    getChunkOfRecipesByCategory(categoryId: string, skip: number, take: number){
        return this.httpClient.get<DataChunk<Recipe>>(`${this.apiUrl}?categoryId=${categoryId}&skip=${skip}&take=${take}`)
    }

    getRecipeById(id: number){
        return this.httpClient.get<Recipe>(`${this.apiUrl}/${id}`)
    }

    addProposedRecipe(proposedRecipe: ProposedRecipe){
        return this.httpClient.post<number>(`${this.apiUrl}/addProposedRecipe`, proposedRecipe);
    }

    getChunkOfProposedRecipes(skip: number, take: number){
        return this.httpClient.get<DataChunk<ProposedRecipe>>(`${this.apiUrl}/proposedRecipes?skip=${skip}&take=${take}`)
    }

    getProposedRecipeById(id: number){
        return this.httpClient.get<ProposedRecipe>(`${this.apiUrl}/proposedRecipes/${id}`);
    }

    getRecipesByIngredients(ingredients: string, skip: number = null, take: number = null){
        return this.httpClient.get<DataChunk<RecipeByIngredient>>(`${this.apiUrl}/byIngredients?ingredients=${ingredients}&skip=${skip}&take=${take}`);
    }

    getCurrentUserRecipes(){
        return this.httpClient.get<Recipe[]>(`${this.apiUrl}/currentUserRecipes`);
    }

    acceptProposedCategory(proposedRecipeId: number, proposedCategory: ProposedCategory){
        return this.httpClient.put(`${this.apiUrl}/acceptProposedCategory/${proposedRecipeId}`, proposedCategory);
    }

    removeProposedCategoryFromProposedRecipe(proposedRecipeId: number, proposedCategoryId: number){
        return this.httpClient.delete(`${this.apiUrl}/removeProposedCategory?proposedRecipeId=${proposedRecipeId}&proposedCategoryId=${proposedCategoryId}`);
    }

    changeProposedIngredientToIngredient(proposedRecipeId: number, proposedIngredientId: number, ingredientId: number){
        return this.httpClient.put(`${this.apiUrl}/changeProposedIngredientToIngredient?proposedRecipeId=${proposedRecipeId}&proposedIngredientId=${proposedIngredientId}&ingredientId=${ingredientId}`, null);
    }

    acceptProposedRecipe(proposedRecipe: ProposedRecipe){
        return this.httpClient.post<number>(`${this.apiUrl}/acceptProposedRecipe`, proposedRecipe);
    }

    discardProposedRecipe(discardRecipeRequest: DiscardRecipeRequest){
        return this.httpClient.delete(`${this.apiUrl}/discardProposedRecipe`, {body: discardRecipeRequest});
    }
}