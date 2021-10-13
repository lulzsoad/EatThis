import { Injectable } from "@angular/core";
import { HttpClient } from '@angular/common/http';
import { AppConfig } from "../app-config/app.config";
import { Ingredient } from "../models/ingredient.model";

@Injectable()
export class IngredientService{
    private serverUrl: string = AppConfig.APP_URL;
    private apiUrl: string = `${this.serverUrl}api/ingredient/`;
    
    constructor(private httpClient: HttpClient){}
    
    getAll(){
        return this.httpClient.get<Ingredient[]>(`${this.apiUrl}`);
    }

    getById(id: number){
        return this.httpClient.get<Ingredient>(`${this.apiUrl}/${id}`);
    }

    add(ingredient: Ingredient){
        return this.httpClient.post<number>(this.apiUrl, ingredient);
    }

    update(ingredient: Ingredient){
        return this.httpClient.put<Ingredient>(this.apiUrl, ingredient);
    }

    delete(ingredient: Ingredient){
        return this.httpClient.delete(this.apiUrl, {body: ingredient});
    }
}