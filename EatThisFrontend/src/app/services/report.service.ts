import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { AppConfig } from "../app-config/app.config";
import { Report } from "../models/report.model";

@Injectable()
export class ReportService{
    private apiUrl = AppConfig.APP_URL + "api/report";
    
    constructor(private httpClient: HttpClient)
    { }

    addReport(report: Report){
        return this.httpClient.post(this.apiUrl, report);
    }

    getCurrentUserReports(){
        return this.httpClient.get<Report[]>(`${this.apiUrl}/currentUser`);
    }
}