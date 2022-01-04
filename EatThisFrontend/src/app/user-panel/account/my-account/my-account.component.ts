import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { Bookmark } from 'src/app/models/bookmark.model';
import { UserDetails } from 'src/app/models/user-details.model';

@Component({
  selector: 'app-my-account',
  templateUrl: './my-account.component.html',
  styleUrls: ['./my-account.component.scss']
})
export class MyAccountComponent implements OnInit {
  public bookmarks: Bookmark[] = [];
  public selectedBookmark: Bookmark;
  public userDetails: UserDetails;

  constructor() { }

  ngOnInit(): void {
    this.initializeBookmarks();
    this.selectedBookmark = this.bookmarks.filter(x => x.isActive)[0];
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

  getUserDetails(){
    this.userDetails = new UserDetails();
    this.userDetails.firstName = "Łukasz"
    this.userDetails.lastName = "Łopata"
    this.userDetails.email = "konieckoncuf@gmail.com"
    this.userDetails.dateOfBirth = new Date("1996-07-01")
    this.userDetails.firstName = "Łukasz"
    

  }
}
