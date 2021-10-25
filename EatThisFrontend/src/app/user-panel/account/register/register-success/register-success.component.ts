import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-register-success',
  templateUrl: './register-success.component.html',
  styleUrls: ['./register-success.component.scss']
})
export class RegisterSuccessComponent implements OnInit {
  email: string | null;

  constructor(private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.email = this.route.snapshot.queryParamMap.get('email');
  }

}
