import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { AppConfig } from "../app-config/app.config";

@Injectable()
export class UserService{
    private apiUrl: string = AppConfig.APP_URL + 'user';

    constructor(private http: HttpClient){

    }

    getUserRole(id: number){
        
    }
}