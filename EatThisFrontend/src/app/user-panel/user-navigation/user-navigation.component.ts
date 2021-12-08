import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { AppInformationService } from 'src/app/services/app-information.service';
import { AuthService } from 'src/app/services/auth.service';
import { IngredientCategoryService } from 'src/app/services/ingredient-category.service';

@Component({
  selector: 'app-user-navigation',
  templateUrl: './user-navigation.component.html',
  styleUrls: ['./user-navigation.component.scss']
})
export class UserNavigationComponent implements OnInit, OnDestroy {
  public isAuthenticated: boolean = false;
  private userSub: Subscription;

  constructor(private authService: AuthService, private appInfoService: AppInformationService) { }

  ngOnInit(): void {
    this.userSub = this.authService.user.subscribe(user => {
      this.isAuthenticated = !user ? false : true;
    });
  }

  ngOnDestroy(): void {
    this.userSub.unsubscribe();
  }

  testRequest(){
    this.appInfoService.getAppInformation().toPromise().then(data => {console.log(data)}).catch(err=> {console.log(err); console.log(err.error)});
  }

  onLogout(){
    this.authService.logout();
  }
}
