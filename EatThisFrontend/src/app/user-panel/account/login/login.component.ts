import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { Router } from '@angular/router';
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

  constructor(private accountService: AccountService, private router: Router) { }

  ngOnInit(): void {
  }

  async logIn(){
    this.loadingPanelVisible = true;
    let isOk: boolean;
    this.login = new Login(this.loginForm.value.email, this.loginForm.value.password)
    await this.accountService.logIn(this.login);
    this.loadingPanelVisible = false;
    this.router.navigate(['../']);
  }
}
