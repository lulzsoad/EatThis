import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { AppConfig } from "../app-config/app.config";
import { DataChunk } from "../models/app-models/data-chunk.model";
import { Operation} from "../models/app-models/patch-operations.model";
import { Role } from "../models/role.model";
import { UserDetails } from "../models/user-details.model";

@Injectable()
export class UserService{
    private apiUrl: string = AppConfig.APP_URL + 'api/user';

    constructor(private http: HttpClient){

    }

    getCurrentUserDetails(){
        return this.http.get<UserDetails>(`${this.apiUrl}/currentUser`);
    }

    patchCurrentUser(operations: Array<Operation>){
        return this.http.patch<UserDetails>(`${this.apiUrl}/currentUser`, operations);
    }

    getChunkOfUsers(skip: number, take: number, search: string){
        return this.http.get<DataChunk<UserDetails>>(`${this.apiUrl}?skip=${skip}&take=${take}&search=${search}`);
    }

    changeUserRole(userId: number, role: Role){
        return this.http.put(`${this.apiUrl}/changeUserRole?userId=${userId}`, role);
    }
}