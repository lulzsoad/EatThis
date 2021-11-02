import { Component, OnInit } from '@angular/core';
import { AccountService } from 'src/app/services/account.service';

@Component({
  selector: 'app-forgot-password',
  templateUrl: './forgot-password.component.html',
  styleUrls: ['./forgot-password.component.scss']
})
export class ForgotPasswordComponent implements OnInit {
  public email: string;

  constructor(private accountService: AccountService) { }

  ngOnInit(): void {
  }

  async sendResetRequest(){
    await this.accountService.sendPasswordResetRequest(this.email);
  }
}
