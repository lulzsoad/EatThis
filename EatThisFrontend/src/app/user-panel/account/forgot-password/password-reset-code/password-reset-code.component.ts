import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-password-reset-code',
  templateUrl: './password-reset-code.component.html',
  styleUrls: ['./password-reset-code.component.scss']
})
export class PasswordResetCodeComponent implements OnInit {
  public code: string = "";
  public email: string;

  constructor(private route: ActivatedRoute, private router: Router) { 
    this.route.queryParams.subscribe(queryParams => {
      this.email = queryParams['email'];
    })
  }

  ngOnInit(): void {
    if(this.email == null || this.email.length < 1){
      this.router.navigate(['../']);
    }
  }

    validate(): boolean{
      if (this.code.length < 10 || this.email == null || this.email.length < 1){
        return true;
      }
      return false;
    }

  submit(){

  }
}
