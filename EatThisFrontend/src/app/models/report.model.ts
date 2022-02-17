import { ReportStatus } from "./report-status.model";

export class Report{
    id: number;
    title: string;
    description: string;
    reportStatus: ReportStatus

    constructor(title: string = null, description: string = null, reportStatus: ReportStatus = null){
        this.title = title,
        this.description = description,
        this.reportStatus = reportStatus;
    }
}