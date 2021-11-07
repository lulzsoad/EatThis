import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { PasswordResetCodeCheckModel } from 'src/app/models/password-reset-code-check.model';
import { PasswordReset } from 'src/app/models/password-reset.model';
import { AccountService } from 'src/app/services/account.service';
import { AlertService } from 'src/app/services/alert.service';

@Component({
  selector: 'app-password-reset-code',
  templateUrl: './password-reset-code.component.html',
  styleUrls: ['./password-reset-code.component.scss']
})
export class PasswordResetCodeComponent implements OnInit {
  public code: string = "";
  public email: string;
  public isPasswordResetCodeCoreect = false;
  public loadingPanelVisible = false;
  public changePasswordForm: FormGroup = new FormGroup({
    newPassword: new FormControl(''),
    confirmPassword: new FormControl('')
  });

  private sr: string;
  private request: PasswordResetCodeCheckModel = new PasswordResetCodeCheckModel();
  private response: PasswordReset = new PasswordReset();


  constructor(private route: ActivatedRoute, private router: Router, private accountService: AccountService, private alertService: AlertService) { 
    this.route.queryParams.subscribe(queryParams => {
      this.email = queryParams['email'];
      this.sr = queryParams['sr'];
    })
  }

  ngOnInit(): void {
    this.checkRoute();
  }

  checkRoute(){
    if(this.email == null || this.email.length < 1){
      this.router.navigate(['../']);
    }
  }

    validate(): boolean{
      if (this.code.length < 1 || this.email == null || this.email.length < 1){
        return true;
      }
      return false;
    }

    async checkPasswordResetCode(){
      this.loadingPanelVisible = true;
      this.request.code = this.code;
      this.request.email = this.email;
      this.response = await this.accountService.checkPasswordResetCode(this.request);
      if(this.response.email != null && this.response.securedRoute != null){
        this.isPasswordResetCodeCoreect = true;
        this.loadingPanelVisible = false;
      }
      this.loadingPanelVisible = false;
  }

  async changePassword(){
    this.loadingPanelVisible = true;
    let validateOk = await this.validateChangePasswordForm();
    if(!validateOk){
      this.loadingPanelVisible = false;
      return;
    }
  
    let response = await this.accountService.changePassword(this.response, this.changePasswordForm.value.newPassword);
    this.router.navigate(['../../']);
    this.loadingPanelVisible = false;
  }

  validateChangePasswordForm(): boolean{
    let passwordWarning = document.querySelector('#passwordWarning') as HTMLElement;

    if(this.changePasswordForm.value.newPassword.length < 6){
      passwordWarning.hidden = false;
      passwordWarning.innerHTML = "Hasło musi mieć minimum 6 znaków";
      passwordWarning.closest('div')?.querySelector('input')?.classList.remove('border-success');
      passwordWarning.closest('div')?.querySelector('input')?.classList.add('border-danger');
      return false;
    }

    if(this.changePasswordForm.value.newPassword != this.changePasswordForm.value.confirmPassword){
      this.alertService.showError("Hasła nie zgadzają się ze sobą")
      return false;
    }

    return true;
  }
}
