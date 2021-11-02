import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router, RouteReuseStrategy } from '@angular/router';
import { AccountService } from 'src/app/services/account.service';

@Component({
  selector: 'app-forgot-password',
  templateUrl: './forgot-password.component.html',
  styleUrls: ['./forgot-password.component.scss']
})
export class ForgotPasswordComponent implements OnInit {
  public email: string = "";
  public loadingPanelVisible: boolean = false;
  
  private requestSent: boolean = false;

  constructor(private accountService: AccountService, private router: Router, private route: ActivatedRoute) { }

  ngOnInit(): void {
  }

  async sendResetRequest(){
    this.loadingPanelVisible = true;
    this.requestSent = await this.accountService.sendPasswordResetRequest(this.email);
    if(this.requestSent){
      this.router.navigate(['code'], {queryParams: {email: this.email}, relativeTo: this.route})
    }
    this.loadingPanelVisible = false;
  }

  validate(){
    if(this.email.length < 5 || !this.email.includes('@') || !this.email.includes('.')){
      return true;
    }
    return false;
  }
}
