import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { AppConfig } from "../app-config/app.config";
import { AuthService } from "./auth.service";
import { exhaustMap, take } from "rxjs/operators";

@Injectable()
export class AppInformationService{
    private apiPath = AppConfig.APP_URL + `api/appinformation`

    constructor(private http: HttpClient, private authService: AuthService){

    }

    getAppInformation(){
        return this.authService.user.pipe(take(1), exhaustMap(user => {
            return this.http.get(this.apiPath);
        }));
        
    };
}