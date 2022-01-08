import { Injectable } from "@angular/core";
import { HttpClient } from '@angular/common/http';
import { AppConfig } from "../app-config/app.config";
import { Subject } from "rxjs";
import { Category } from "../models/category.model";
import { AlertService } from "./app-services/alert.service";

@Injectable()
export class CategoryService{
    public createCategoryModalSaved: Subject<Category> = new Subject<Category>();

    private serverUrl: string = AppConfig.APP_URL;
    private apiUrl: string = `${this.serverUrl}api/category/`;
    
    constructor(private httpClient: HttpClient, private alertService: AlertService){}
    
    async getAll(): Promise<Category[]>{
        let result: Category[] = [];
        await this.httpClient.get<Category[]>(`${this.apiUrl}`)
            .toPromise()
            .then(data => result = data)
            .catch((err) => this.alertService.showError(err.error));
        return result;
    }

    async getById(id: number){
        let category: Category = new Category();
        await this.httpClient.get<Category>(`${this.apiUrl}/${id}`)
            .toPromise()
            .then(data => category = data)
            .catch(err => this.alertService.showError(err.error));
        return category;
    }

    async add(category: Category){
        let id: number;
        await this.httpClient.post<number>(this.apiUrl, category)
            .toPromise()
            .then(data => {
                id = data;
                this.alertService.showSuccess("Sukces");
            })
            .catch(err => this.alertService.showError(err.error));
    }

    async update(category: Category){
        await this.httpClient.put<Category>(this.apiUrl, category)
            .toPromise()
            .then(data => {category = data; this.alertService.showSuccess("Sukces")})
            .catch(err => this.alertService.showError(err.error));
        return category;
    }

    async delete(category: Category){
        await this.httpClient.delete(this.apiUrl, {body: category})
            .toPromise()
            .then(() => this.alertService.showSuccess("Sukces"))
            .catch(err => this.alertService.showError(err.error));
    }
}