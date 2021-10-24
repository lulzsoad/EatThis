import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { AppConfig } from "../app-config/app.config";
import { RegisterUser } from "../models/register-user.model";
import { AlertService } from "./alert.service";

@Injectable()
export class AccountService{
    private serverUrl: string = AppConfig.APP_URL;
    private apiUrl: string = `${this.serverUrl}api/account/`;

    constructor(private httpClient: HttpClient, private alertService: AlertService){

    }

    async register(registerUser: RegisterUser){
        let isOk: Boolean = false;
        await this.httpClient.post<RegisterUser>(`${this.apiUrl}register`, registerUser)
        .toPromise()
        .then(() => {
            isOk = true;
        })
        .catch((err) => {
            this.alertService.showError(err.error);
            isOk = false;;
        })
        return isOk;
    }
}