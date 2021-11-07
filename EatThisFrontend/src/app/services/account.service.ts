import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { AppConfig } from "../app-config/app.config";
import { PasswordResetCodeCheckModel } from "../models/password-reset-code-check.model";
import { PasswordResetCodeNewPassword } from "../models/password-reset-code-new-password.model";
import { PasswordReset } from "../models/password-reset.model";
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
            });
        return isOk;
    }

    async activateAccount(activationCode: string){
        let result: boolean = false;
        await this.httpClient.get<boolean>(`${this.apiUrl}activate/${activationCode}`)
            .toPromise()
            .then(data => result = data)
            .catch(err => {
                this.alertService.showError(err.error);
            });
        return result;
    }

    async sendPasswordResetRequest(email: string){
        let passwordResetModel: PasswordReset = new PasswordReset();

        await this.httpClient.get<PasswordReset>(`${this.apiUrl}forgotten-password?email=${email}`)
        .toPromise()
        .then(data => passwordResetModel = data)
        .catch(err => {
            this.alertService.showError(err.error);
        })

        return passwordResetModel;
    }

    async checkPasswordResetCode(request: PasswordResetCodeCheckModel){
        let response: PasswordReset = new PasswordReset();
        await this.httpClient.post<PasswordReset>(`${this.apiUrl}check-reset-code`, request)
        .toPromise()
        .then(data => response = data)
        .catch(err => {this.alertService.showError(err.error); return;});
        return response;
    } 

    async changePassword(passwordReset: PasswordReset, newPassword: string){
        let response: boolean = false;
        console.log(passwordReset);
        console.log(newPassword);

        let request = new PasswordResetCodeNewPassword()
        request.passwordResetCode = passwordReset;
        request.password = newPassword;
        console.log(request);

        await this.httpClient.post(`${this.apiUrl}change-password-reset-code`, request)
        .toPromise()
        .then(() => {
            response = true;
            this.alertService.showSuccess("Hasło zostało zmienione");
        })
        .catch((err) => this.alertService.showError(err.error))

        return response;
    }
}