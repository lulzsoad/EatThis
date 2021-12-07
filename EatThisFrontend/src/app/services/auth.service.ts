import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { AppConfig } from "../app-config/app.config";
import { RegisterUser } from "../models/register-user.model";
import { catchError } from "rxjs/operators";
import { BehaviorSubject, Subject, throwError } from "rxjs";
import { AlertService } from "./alert.service";
import { Login } from "../models/login.model";
import { AuthToken } from "../models/auth-token.model";
import { User } from "../models/user.model";

@Injectable()
export class AuthService{
    public user: BehaviorSubject<User> = new BehaviorSubject<User>(null);

    private serverUrl: string = AppConfig.APP_URL;
    private apiUrl: string = `${this.serverUrl}api/account/`;

    constructor(private http: HttpClient, private alertService: AlertService){

    }

    async signup(newUser: RegisterUser){
        let isOk: Boolean = false;
        await this.http.post<RegisterUser>(`${this.apiUrl}register`, newUser)
            .toPromise()
            .then(() => {
                isOk = true;
            })
            .catch((err) => {
                this.alertService.showError(err.error);
                isOk = false;;
            });
        return isOk;
    }

    async logIn(login: Login){
        let isOk = false;
        await this.http.post<AuthToken>(`${this.apiUrl}login`, login)
            .toPromise()
            .then((response) => {
                const expirationDate = response.tokenExpirationDate;
                this.handleAuthentication(response.email, response.userId, response.token, expirationDate)
                this.alertService.showSuccess("Zalogowano pomyślnie");
                isOk = true;
            })
            .catch((err) => this.alertService.showError(err.error));
        return isOk;
    }

    private handleAuthentication(email: string, userId: string, token: string, tokenExpirationDate: Date){
        const user = new User(email, userId, token, tokenExpirationDate)
        this.user.next(user);
        localStorage.setItem("token", user.token);
    }
}