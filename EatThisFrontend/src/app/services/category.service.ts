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
    
    getAll(){
        return this.httpClient.get<Category[]>(`${this.apiUrl}`);
    }

    getById(id: number){
        return this.httpClient.get<Category>(`${this.apiUrl}/${id}`)
    }

    add(category: Category){
        return this.httpClient.post<number>(this.apiUrl, category);
    }

    update(category: Category){
        return this.httpClient.put<Category>(this.apiUrl, category);
    }

    delete(category: Category){
        return this.httpClient.delete(this.apiUrl, {body: category});
    }
}