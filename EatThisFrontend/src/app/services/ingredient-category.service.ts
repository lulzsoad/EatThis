import { Injectable } from "@angular/core";
import { HttpClient } from '@angular/common/http';
import { AppConfig } from "../app-config/app.config";
import { Subject } from "rxjs";
import { AlertService } from "./alert.service";
import { IngredientCategory } from "../models/ingredient-category.model";

@Injectable()
export class IngredientCategoryService{
    public createIngredientCategoryModalSaved: Subject<IngredientCategory> = new Subject<IngredientCategory>();

    private serverUrl: string = AppConfig.APP_URL;
    private apiUrl: string = `${this.serverUrl}api/IngredientCategory/`;
    
    constructor(private httpClient: HttpClient, private alertService: AlertService){}
    
    async getAll(): Promise<IngredientCategory[]>{
        let result: IngredientCategory[] = [];
        await this.httpClient.get<IngredientCategory[]>(`${this.apiUrl}`)
            .toPromise()
            .then(data => result = data)
            .catch((err) => this.alertService.showError(err.error));
        return result;
    }

    async getById(id: number){
        let ingredientCategory: IngredientCategory = new IngredientCategory();
        await this.httpClient.get<IngredientCategory>(`${this.apiUrl}/${id}`)
            .toPromise()
            .then(data => ingredientCategory = data)
            .catch(err => this.alertService.showError(err.error));
        return ingredientCategory;
    }

    async add(ingredientCategory: IngredientCategory){
        let id: number;
        await this.httpClient.post<number>(this.apiUrl, ingredientCategory)
            .toPromise()
            .then(data => {
                id = data;
                this.alertService.showSuccess("Sukces");
            })
            .catch(err => this.alertService.showError(err.error));
    }

    async update(ingredientCategory: IngredientCategory){
        await this.httpClient.put<IngredientCategory>(this.apiUrl, ingredientCategory)
            .toPromise()
            .then(data => {ingredientCategory = data; this.alertService.showSuccess("Sukces")})
            .catch(err => this.alertService.showError(err.error));
        return ingredientCategory;
    }

    async delete(ingredientCategory: IngredientCategory){
        await this.httpClient.delete(this.apiUrl, {body: ingredientCategory})
            .toPromise()
            .then(() => this.alertService.showSuccess("Sukces"))
            .catch(err => this.alertService.showError(err.error));
    }
}