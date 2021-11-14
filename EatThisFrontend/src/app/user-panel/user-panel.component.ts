import { Component, OnDestroy, OnInit } from '@angular/core';

@Component({
  selector: 'app-user-panel',
  templateUrl: './user-panel.component.html',
  styleUrls: ['./user-panel.component.scss']
})
export class UserPanelComponent implements OnInit{
  public isAuthenticated = false;

  constructor() { }
  
  
  ngOnInit(): void {

  }

}
