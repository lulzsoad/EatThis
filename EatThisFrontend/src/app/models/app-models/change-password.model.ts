export class ChangePassword{
    oldPassword: string;
    newPassword: string;

    constructor(oldPassword: string, newPassword: string){
        this.newPassword = newPassword;
        this.oldPassword = oldPassword;
    }
}