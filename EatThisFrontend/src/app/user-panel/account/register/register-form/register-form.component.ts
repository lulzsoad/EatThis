import { Component, ElementRef, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { RegisterUser } from 'src/app/models/register-user.model';
import { AccountService } from 'src/app/services/account.service';

@Component({
  selector: 'app-register-form',
  templateUrl: './register-form.component.html',
  styleUrls: ['./register-form.component.scss']
})
export class RegisterFormComponent implements OnInit {

  public registerUser: RegisterUser;
  public formGroup: FormGroup = new FormGroup({
    email: new FormControl('', [Validators.required, Validators.minLength(5), Validators.email]),
    password: new FormControl('', [Validators.required, Validators.minLength(6)]),
    confirmPassword: new FormControl('', [Validators.required, Validators.minLength(6)]),
    firstName: new FormControl('', [Validators.required]),
    lastName: new FormControl('', [Validators.required]),
    birthDate: new FormControl()
  });
  public form: ElementRef;
  public loadingPanelVisible = false;

  constructor(private accountService: AccountService, private router: Router, private route: ActivatedRoute) { }

  ngOnInit(): void {
  }

  async register(){
    if(!this.validateForm()){
      return;
    }
    this.loadingPanelVisible = true;
    let registerUser: RegisterUser = this.formGroup.value;
    let registerSucceed = await this.accountService.register(registerUser);
    if(!registerSucceed){
      this.loadingPanelVisible = false;
      return;
    }
    this.loadingPanelVisible = false;
    this.router.navigate(['./success'], {relativeTo: this.route, queryParams: {email: registerUser.email}});
  }

  validateForm(): boolean{
    let form = document.querySelector('form') as HTMLFormElement;
    let inputs = form.getElementsByTagName("input");
    let isOk = true;
    for(let i = 0; i < inputs.length; i++){
      inputs.item(i)?.dispatchEvent(new FocusEvent('focusout'));
      if(inputs.item(i)?.className.includes('border-danger')){
        isOk = false;
      }
    }

    let passwordWarning = document.querySelector('#passwordWarning') as HTMLElement;
    let confirmPasswordWarning = document.querySelector('#confirmPasswordWarning') as HTMLElement;
    console.log(passwordWarning);
    if(this.formGroup.value.password.length < 6){
      passwordWarning.hidden = false;
      passwordWarning.innerHTML = "Hasło musi mieć minimum 6 znaków";
      passwordWarning.closest('div')?.querySelector('input')?.classList.remove('border-success');
      passwordWarning.closest('div')?.querySelector('input')?.classList.add('border-danger');
      isOk = false;
    }
    if(this.formGroup.value.password != this.formGroup.value.confirmPassword){
      confirmPasswordWarning.hidden = false;
      confirmPasswordWarning.innerHTML = "Hasła nie są ze sobą zgodne";
      confirmPasswordWarning.closest('div')?.querySelector('input')?.classList.remove('border-success');
      confirmPasswordWarning.closest('div')?.querySelector('input')?.classList.add('border-danger');
      isOk = false;
    }

    return isOk;
  }

}
