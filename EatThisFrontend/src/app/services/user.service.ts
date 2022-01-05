import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { AppConfig } from "../app-config/app.config";
import { UserDetails } from "../models/user-details.model";

@Injectable()
export class UserService{
    private apiUrl: string = AppConfig.APP_URL + 'api/user';

    constructor(private http: HttpClient){

    }

    getCurrentUserDetails(){
        return this.http.get<UserDetails>(`${this.apiUrl}/currentUser`);
    }
}