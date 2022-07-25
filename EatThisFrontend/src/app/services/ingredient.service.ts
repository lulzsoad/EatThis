import { Injectable } from "@angular/core";
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { AppConfig } from "../app-config/app.config";
import { Ingredient } from "../models/ingredient.model";
import { Subject } from "rxjs";
import { AlertService } from "./app-services/alert.service";
import { IngredientPosition } from "../models/app-models/ingredient-position.model";
import { IngredientQuantity } from "../models/ingredient-quantity.model";
import { IngredientQuantitiesVM } from "../models/app-models/ingredient-quantities-vm.model";
import { AuthService } from "./auth.service";
import { User } from "../models/user.model";

@Injectable()
export class IngredientService{
    public createIngredientModalSaved: Subject<Ingredient> = new Subject<Ingredient>();
    public addIngredientModal: Subject<IngredientPosition> = new Subject<IngredientPosition>();
    public proposeIngredientModal: Subject<IngredientPosition> = new Subject<IngredientPosition>();

    private serverUrl: string = AppConfig.APP_URL;
    private apiUrl: string = `${this.serverUrl}api/ingredient/`;
    
    constructor(private httpClient: HttpClient, private alertService: AlertService, private authService: AuthService){}
    private user: User
    
    getAll(){
        return this.httpClient.get<Ingredient[]>(`${this.apiUrl}`);
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

    update(ingredient: Ingredient){
        return this.httpClient.put<Ingredient>(this.apiUrl, ingredient)
    }

    async delete(ingredient: Ingredient){
        await this.httpClient.delete(this.apiUrl, {body: ingredient})
            .toPromise()
            .then(() => this.alertService.showSuccess("Sukces"))
            .catch(err => this.alertService.showError(err.error));
    }

    acceptProposedIngredient(id: number){
        return this.httpClient.get<IngredientQuantity>(`${this.apiUrl}accept/${id}`)
    }

    getIngredientQuantietiesByProposedRecipeId(id: number){
        return this.httpClient.get<IngredientQuantitiesVM>(`${this.apiUrl}proposedRecipeIngredientQuantities/${id}`);
    }

    deleteProposedIngredient(id:number){
        console.log(id);
        return this.httpClient.delete(`${this.apiUrl}proposedIngredient/${id}`);
    } 
}
