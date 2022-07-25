import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { BehaviorSubject, Subject } from "rxjs";
import { AppConfig } from "../app-config/app.config";
import { ChangePassword } from "../models/app-models/change-password.model";
import { PasswordResetCodeCheckModel } from "../models/password-reset-code-check.model";
import { PasswordResetCodeNewPassword } from "../models/password-reset-code-new-password.model";
import { PasswordReset } from "../models/password-reset.model";
import { Role } from "../models/role.model";
import { User } from "../models/user.model";
import { AlertService } from "./app-services/alert.service";


@Injectable()
export class AccountService{
    public user: BehaviorSubject<User> = new BehaviorSubject<User>(null);
    
    private serverUrl: string = AppConfig.APP_URL;
    private apiUrl: string = `${this.serverUrl}api/account/`;
    

    constructor(private httpClient: HttpClient, private alertService: AlertService){

    }

    activateAccount(activationCode: string){
        return this.httpClient.get<boolean>(`${this.apiUrl}activate/${activationCode}`)
    }

    sendPasswordResetRequest(email: string){
        return this.httpClient.get<PasswordReset>(`${this.apiUrl}forgotten-password?email=${email}`);
    }

    checkPasswordResetCode(request: PasswordResetCodeCheckModel){
        return this.httpClient.post<PasswordReset>(`${this.apiUrl}check-reset-code`, request);
    } 

    changePassword(passwordReset: PasswordReset, newPassword: string){
        let request = new PasswordResetCodeNewPassword()
        request.passwordResetCode = passwordReset;
        request.password = newPassword;
        console.log("asd");
        console.log(this.httpClient.post(`${this.apiUrl}change-password-reset-code`, request));
        return this.httpClient.post(`${this.apiUrl}change-password-reset-code`, request);
    }

    changePasswordModal(changePasswordVm: ChangePassword){
        return this.httpClient.put(`${this.apiUrl}change-password`, changePasswordVm);
    }

    getRoles(){
        return this.httpClient.get<Role[]>(`${this.apiUrl}roles`);
    }
}