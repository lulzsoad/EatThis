import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { AuthToken } from 'src/app/models/auth-token.model';
import { Login } from 'src/app/models/login.model';
import { AccountService } from 'src/app/services/account.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  public loginForm = new FormGroup({
    email: new FormControl(''),
    password: new FormControl('')
  });
  public loadingPanelVisible = false;

  private login: Login;
  private token: AuthToken;

  constructor(private accountService: AccountService) { }

  ngOnInit(): void {
  }

  async logIn(){
    this.loadingPanelVisible = true;
    this.login = new Login(this.loginForm.value.email, this.loginForm.value.password)
    this.token = await this.accountService.logIn(this.login);
    this.loadingPanelVisible = false;
  }
}
