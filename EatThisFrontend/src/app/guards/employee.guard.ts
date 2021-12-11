import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, Router, RouterStateSnapshot, UrlTree } from "@angular/router";
import { Observable } from "rxjs";
import { map, take } from "rxjs/operators";
import { RoleEnum } from "../enums/role-enum.enum";
import { AuthService } from "../services/auth.service";

@Injectable()
export class EmployeeGuard{
    constructor(private authService: AuthService, private router: Router){}

    canActivate(route: ActivatedRouteSnapshot, router: RouterStateSnapshot): boolean | Promise<boolean> | Observable<boolean | UrlTree>{
        return this.authService.user.pipe(
            take(1),
            map(user => {
                const isEmployee = user.roleId == RoleEnum.EMPLOYEE;
                const isAdmin = user.roleId == RoleEnum.ADMIN;
                if(isEmployee || isAdmin){
                    return true;
                }

                return this.router.createUrlTree(['/']);
            }));
    }
}