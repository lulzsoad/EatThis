import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AccountService } from 'src/app/services/account.service';
import { AlertService } from 'src/app/services/alert.service';

@Component({
  selector: 'app-activate',
  templateUrl: './activate.component.html',
  styleUrls: ['./activate.component.scss']
})
export class ActivateComponent implements OnInit {
  public loadingPanelVisible: boolean;
  private activationCode: string;

  constructor(private accountService: AccountService, private route: ActivatedRoute, private router: Router, private alertService: AlertService) { 
    this.route.queryParams.subscribe(queryParams => {
      this.activationCode = queryParams['activationCode']
    })
  }

  async ngOnInit(): Promise<void> {
    this.loadingPanelVisible = true;

    await this.activateAccount(this.activationCode);

    this.loadingPanelVisible = false;
  }

  private async activateAccount(activationCode: string){
    let isActivated = await this.accountService.activateAccount(activationCode);
    if(isActivated){
      this.alertService.showSuccess("Konto zostało aktywowane");
    }
    this.router.navigate(['../../login'], {relativeTo: this.route})
  }
}
