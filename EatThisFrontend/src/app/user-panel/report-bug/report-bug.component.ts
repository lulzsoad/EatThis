import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ConfigStore } from 'src/app/app-config/config-store';
import { Report } from 'src/app/models/report.model';
import { AlertService } from 'src/app/services/app-services/alert.service';
import { ReportService } from 'src/app/services/report.service';

@Component({
  selector: 'app-report-bug',
  templateUrl: './report-bug.component.html',
  styleUrls: ['./report-bug.component.scss']
})
export class ReportBugComponent implements OnInit {
  public report: Report = new Report();

  constructor(
    private alertService: AlertService,
    private configStore: ConfigStore,
    private reportService: ReportService,
    private router: Router
  ) { }

  ngOnInit(): void {
  }

  async addReport(){
    let validated: boolean = this.validate();
    if(validated){
      this.configStore.startLoadingPanel();
      await this.reportService.addReport(this.report).toPromise();
      this.configStore.stopLoadingPanel();
      this.alertService.showInfo("Zgłoszono do administracji");
      this.router.navigate(['']);
    }
  }

  validate(): boolean{
    if(this.report.title.length < 5){
      this.alertService.showError("Tytuł musi mieć minimum 5 znaków");
      return false;
    }else if(this.report.description.length < 10){
      this.alertService.showError("Opis musi mieć minimum 10 znaków");
      return false;
    }
    return true;
  }
}
