import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, Router, RouterStateSnapshot, UrlTree } from "@angular/router";
import { Observable } from "rxjs";
import { map, take } from "rxjs/operators";
import { RoleEnum } from "../enums/role-enum.enum";
import { AlertService } from "../services/app-services/alert.service";
import { AuthService } from "../services/auth.service";

@Injectable()
export class AdminGuard{
    constructor(private authService: AuthService, private router: Router, private alertService: AlertService){}

    canActivate(route: ActivatedRouteSnapshot, router: RouterStateSnapshot): boolean | Promise<boolean> | Observable<boolean | UrlTree>{
        return this.authService.user.pipe(
            take(1),
            map(user => {
                const isAdmin = user.roleId == RoleEnum.ADMIN;
                if(isAdmin){
                    return true;
                }
                
                this.alertService.showError("Nie masz uprawnień do przeglądania tej zawartości");
                return false;
            }));
    }
}