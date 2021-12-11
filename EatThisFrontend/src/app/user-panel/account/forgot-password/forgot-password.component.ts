import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router, RouteReuseStrategy } from '@angular/router';
import { ConfigStore } from 'src/app/app-config/config-store';
import { PasswordReset } from 'src/app/models/password-reset.model';
import { AccountService } from 'src/app/services/account.service';

@Component({
  selector: 'app-forgot-password',
  templateUrl: './forgot-password.component.html',
  styleUrls: ['./forgot-password.component.scss']
})
export class ForgotPasswordComponent implements OnInit {
  public email: string = "";
  
  private requestSent: boolean = false;
  private passwordReset: PasswordReset;

  constructor(private accountService: AccountService, private router: Router, private route: ActivatedRoute, private configStore: ConfigStore) { }

  ngOnInit(): void {
  }

  async sendResetRequest(){
    this.configStore.startLoadingPanel();
    this.passwordReset = await this.accountService.sendPasswordResetRequest(this.email);
    if(this.passwordReset.email != null){
      this.router.navigate(['code'], {queryParams: {email: this.passwordReset.email}, relativeTo: this.route})
    }
    this.configStore.stopLoadingPanel();
  }

  validate(){
    if(this.email.length < 5 || !this.email.includes('@') || !this.email.includes('.')){
      return true;
    }
    return false;
  }
}
