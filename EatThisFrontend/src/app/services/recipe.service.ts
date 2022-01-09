import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { AppConfig } from "../app-config/app.config";
import { Recipe } from "../models/recipe.model";

@Injectable()
export class RecipeService{
    private apiUrl = `${AppConfig.APP_URL}api/recipe`;

    constructor(private httpClient: HttpClient)
    { }

    addRecipe(recipe: Recipe){
        return this.httpClient.post<number>(this.apiUrl, recipe);
    }

    getRecipesByCategory(categoryId: string, skip: number, take: number){
        return this.httpClient.get<Recipe[]>(`${this.apiUrl}?categoryId=${categoryId}&skip=${skip}&take=${take}`)
    }
}