import { Injectable } from "@angular/core";
import { HttpClient } from '@angular/common/http';
import { AppConfig } from "../app-config/app.config";
import { Unit } from "../models/unit.model";
import { Subject } from "rxjs";

@Injectable()
export class UnitService{
    public createUnitModalSaved: Subject<Unit> = new Subject<Unit>();

    private serverUrl: string = AppConfig.APP_URL;
    private apiUrl: string = `${this.serverUrl}api/unit/`;
    
    constructor(private httpClient: HttpClient){}
    
    getAll(){
        return this.httpClient.get<Unit[]>(`${this.apiUrl}`);
    }

    getById(id: number){
        return this.httpClient.get<Unit>(`${this.apiUrl}/${id}`);
    }

    add(unit: Unit){
        return this.httpClient.post<number>(this.apiUrl, unit);
    }

    update(unit: Unit){
        return this.httpClient.put<Unit>(this.apiUrl, unit);
    }

    delete(unit: Unit){
        return this.httpClient.delete(this.apiUrl, {body: unit});
    }
}