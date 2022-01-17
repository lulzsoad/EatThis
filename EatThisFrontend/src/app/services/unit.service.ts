import { Injectable } from "@angular/core";
import { HttpClient } from '@angular/common/http';
import { AppConfig } from "../app-config/app.config";
import { Unit } from "../models/unit.model";
import { Subject } from "rxjs";
import { AlertService } from "./app-services/alert.service";

@Injectable()
export class UnitService{
    public createUnitModalSaved: Subject<Unit> = new Subject<Unit>();

    private serverUrl: string = AppConfig.APP_URL;
    private apiUrl: string = `${this.serverUrl}api/unit/`;
    
    constructor(private httpClient: HttpClient, private alertService: AlertService){}
    
    getAll(){
        return this.httpClient.get<Unit[]>(`${this.apiUrl}`);
    }

    async getById(id: number){
        let unit: Unit = new Unit();
        await this.httpClient.get<Unit>(`${this.apiUrl}/${id}`)
            .toPromise()
            .then(data => unit = data)
            .catch(err => this.alertService.showError(err.error));
        return unit;
    }

    async add(unit: Unit){
        let id: number;
        await this.httpClient.post<number>(this.apiUrl, unit)
            .toPromise()
            .then(data => {
                id = data;
                this.alertService.showSuccess("Sukces");
            })
            .catch(err => this.alertService.showError(err.error));
    }

    async update(unit: Unit){
        await this.httpClient.put<Unit>(this.apiUrl, unit)
            .toPromise()
            .then(data => {unit = data; this.alertService.showSuccess("Sukces")})
            .catch(err => this.alertService.showError(err.error));
        return unit;
    }

    async delete(unit: Unit){
        await this.httpClient.delete(this.apiUrl, {body: unit})
            .toPromise()
            .then(() => this.alertService.showSuccess("Sukces"))
            .catch(err => this.alertService.showError(err.error));
    }
}