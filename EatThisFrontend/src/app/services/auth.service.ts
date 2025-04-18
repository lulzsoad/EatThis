import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { AppConfig } from "../app-config/app.config";
import { RegisterUser } from "../models/register-user.model";
import { catchError } from "rxjs/operators";
import { BehaviorSubject, Subject, throwError } from "rxjs";
import { AlertService } from "./app-services/alert.service";
import { Login } from "../models/login.model";
import { AuthToken } from "../models/auth-token.model";
import { User } from "../models/user.model";
import { Router } from "@angular/router";

@Injectable()
export class AuthService{
    public user: BehaviorSubject<User> = new BehaviorSubject<User>(null);

    private serverUrl: string = AppConfig.APP_URL;
    private apiUrl: string = `${this.serverUrl}api/account/`;
    private tokenExpirationTimer: any;

    constructor(private http: HttpClient, private alertService: AlertService, private router: Router){

    }

    signup(newUser: RegisterUser){
        return this.http.post<RegisterUser>(`${this.apiUrl}register`, newUser)
    }

    logIn(login: Login){
        return this.http.post<AuthToken>(`${this.apiUrl}login`, login);
    }

    public handleAuthentication(email: string, userId: string, roleId: number, token: string, tokenExpirationDate: Date){
        const user = new User(email, userId, roleId, token, tokenExpirationDate)
        this.user.next(user);
        const expirationDuration = new Date(tokenExpirationDate).getTime() - new Date().getTime();
        this.autoLogOut(expirationDuration);
        localStorage.setItem("userData", JSON.stringify(user));
    }

    logout(){
        this.user.next(null);
        this.alertService.showInfo("Wylogowano");
        this.router.navigate(['/login']);
        localStorage.removeItem('userData');
        if(this.tokenExpirationTimer){
            clearTimeout(this.tokenExpirationTimer);
        }
        this.tokenExpirationTimer = null;
    }

    autoLogIn(){
        const userData: {
            email: string, 
            id: string, 
            _roleId: number,
            _token: string, 
            _tokenExpirationDate: string
        } = JSON.parse(localStorage.getItem('userData'));
        
        if(!userData){
            return;
        }

        const loadedUser = new User(
            userData.email, 
            userData.id, 
            userData._roleId,
            userData._token, 
            new Date(userData._tokenExpirationDate)
        );

        if(loadedUser.token) {
            this.user.next(loadedUser);
            const expirationDuration = new Date(userData._tokenExpirationDate).getTime() - new Date().getTime();
            this.autoLogOut(expirationDuration);
        }
    }

    autoLogOut(expirationDuration: number){
        this.tokenExpirationTimer = setTimeout(() => {
            this.logout();
        }, expirationDuration)
    }
}