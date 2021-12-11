import { RoleEnum } from "../enums/role-enum.enum";
import { User } from "./user.model";

export class AuthInfo{
    isAuthenticated: boolean;
    isEmployee: boolean;
    isAdmin: boolean;

    constructor(user: User){
        console.log(user);
        this.isAuthenticated = !user ? false : true;
        this.isEmployee = (this.isAuthenticated && user.roleId) == RoleEnum.EMPLOYEE ? true : false;
        this.isAdmin = (this.isAuthenticated && user.roleId) == RoleEnum.ADMIN ? true: false;
    }
}
