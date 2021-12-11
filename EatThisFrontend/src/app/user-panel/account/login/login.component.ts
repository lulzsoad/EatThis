import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { Router } from '@angular/router';
import { ConfigStore } from 'src/app/app-config/config-store';
import { AuthToken } from 'src/app/models/auth-token.model';
import { Login } from 'src/app/models/login.model';
import { AccountService } from 'src/app/services/account.service';
import { AuthService } from 'src/app/services/auth.service';

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

  private login: Login;

  constructor(private accountService: AccountService, private router: Router, private authService: AuthService, private configStore: ConfigStore) { }

  ngOnInit(): void {
  }

  async logIn(){
    this.configStore.startLoadingPanel();
    let isOk: boolean;
    this.login = new Login(this.loginForm.value.email, this.loginForm.value.password)
    isOk = await this.authService.logIn(this.login);
    this.configStore.stopLoadingPanel();
    if(isOk){
      this.router.navigate(['../']);
    }
  }
}
