import { Injectable } from "@angular/core";
import { HttpClient } from '@angular/common/http';
import { AppConfig } from "../app-config/app.config";
import { Ingredient } from "../models/ingredient.model";
import { Subject } from "rxjs";
import { AlertService } from "./app-services/alert.service";

@Injectable()
export class IngredientService{
    public createIngredientModalSaved: Subject<Ingredient> = new Subject<Ingredient>();

    private serverUrl: string = AppConfig.APP_URL;
    private apiUrl: string = `${this.serverUrl}api/ingredient/`;
    
    constructor(private httpClient: HttpClient, private alertService: AlertService){}
    
    async getAll(): Promise<Ingredient[]>{
        let result: Ingredient[] = [];
        await this.httpClient.get<Ingredient[]>(`${this.apiUrl}`)
            .toPromise()
            .then(data => result = data)
            .catch((err) => this.alertService.showError(err.error));
        return result;
    }

    async getById(id: number){
        let ingredient: Ingredient = new Ingredient();
        await this.httpClient.get<Ingredient>(`${this.apiUrl}/${id}`)
            .toPromise()
            .then(data => ingredient = data)
            .catch(err => this.alertService.showError(err.error));
        return ingredient;
    }

    async add(ingredient: Ingredient){
        let id: number;
        await this.httpClient.post<number>(this.apiUrl, ingredient)
            .toPromise()
            .then(data => {
                id = data;
                this.alertService.showSuccess("Sukces");
            })
            .catch(err => this.alertService.showError(err.error));
    }

    async update(ingredient: Ingredient){
        await this.httpClient.put<Ingredient>(this.apiUrl, ingredient)
            .toPromise()
            .then(data => {ingredient = data; this.alertService.showSuccess("Sukces")})
            .catch(err => this.alertService.showError(err.error));
        return ingredient;
    }

    async delete(ingredient: Ingredient){
        await this.httpClient.delete(this.apiUrl, {body: ingredient})
            .toPromise()
            .then(() => this.alertService.showSuccess("Sukces"))
            .catch(err => this.alertService.showError(err.error));
    }
}
