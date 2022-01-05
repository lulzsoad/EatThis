import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { ConfigStore } from 'src/app/app-config/config-store';
import { RoleEnum } from 'src/app/enums/role-enum.enum';
import { Bookmark } from 'src/app/models/bookmark.model';
import { UserDetails } from 'src/app/models/user-details.model';
import { AlertService } from 'src/app/services/alert.service';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-my-account',
  templateUrl: './my-account.component.html',
  styleUrls: ['./my-account.component.scss']
})
export class MyAccountComponent implements OnInit {
  public bookmarks: Bookmark[] = [];
  public selectedBookmark: Bookmark;
  public userDetails: UserDetails;

  public role = RoleEnum;

  constructor(
    private userService: UserService,
    private configStore: ConfigStore,
    private alertService: AlertService
    ) { }

  async ngOnInit(): Promise<void> {
    this.configStore.startLoadingPanel();
    await this.getUserDetails();
    this.initializeBookmarks();
    this.selectedBookmark = this.bookmarks.filter(x => x.isActive)[0];
    this.configStore.stopLoadingPanel();
  }

  changeBookmark(bookmark: Bookmark){
    if(bookmark.isDisabled)
      return;

    this.bookmarks.filter(x => x.isActive)[0].isActive = false;
    this.bookmarks.filter(x => x.name == bookmark.name)[0].isActive = true;
    this.selectedBookmark = bookmark;
  }

  initializeBookmarks(){
    this.bookmarks.push(new Bookmark("MyRecipes", "Moje przepisy", true));
    this.bookmarks.push(new Bookmark("MyReports", "Moje zgłoszenia", false));
    this.bookmarks.push(new Bookmark("AdditionalServices", "Usługi dodatkowe", false, true));
  }

  async getUserDetails(){
    this.userDetails = await this.userService.getCurrentUserDetails().toPromise()

    console.log(this.userDetails);
  }
}
