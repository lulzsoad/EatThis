import { Component, ElementRef, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { RegisterUser } from 'src/app/models/register-user.model';
import { AccountService } from 'src/app/services/account.service';
import { isNamedImports, validateLocaleAndSetLanguage } from 'typescript';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {
  
  constructor(){

  }
  
  ngOnInit(): void {
    
  }
 
}
