import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { AccountService } from '../services/account.service';

@Component({
  selector: 'app-user-panel',
  templateUrl: './user-panel.component.html',
  styleUrls: ['./user-panel.component.scss']
})
export class UserPanelComponent implements OnInit, OnDestroy {
  public isAuthenticated = false;

  private userSubscription: Subscription;

  constructor(private accountService: AccountService) { }
  
  
  ngOnInit(): void {
    this.userSubscription = this.accountService.user.subscribe(user => {
      console.log('asd');
      this.isAuthenticated = !!user // jednoznaczne z: !user ? false : true;
      console.log(this.isAuthenticated);
    });
  }

  ngOnDestroy(): void {
    this.userSubscription.unsubscribe();
  }

}
