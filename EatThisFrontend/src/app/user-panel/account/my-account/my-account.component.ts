import { Component, OnInit} from '@angular/core';
import { ConfigStore } from 'src/app/app-config/config-store';
import { RoleEnum } from 'src/app/enums/role-enum.enum';
import { Operation, OperationEnum} from 'src/app/models/app-models/patch-operations.model';
import { Bookmark } from 'src/app/models/bookmark.model';
import { UserDetails } from 'src/app/models/user-details.model';
import { AlertService } from 'src/app/services/alert.service';
import { DateFormat } from 'src/app/services/app-services/date.service';
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
  public userImage: string = "";
  public editMode: boolean = false;
  public DateFormat = DateFormat;

  public role = RoleEnum;

  constructor(
    private userService: UserService,
    private configStore: ConfigStore,
    private alertService: AlertService
    ) { }

  async ngOnInit(): Promise<void> {
    this.initializeBookmarks();
    this.selectedBookmark = this.bookmarks.filter(x => x.isActive)[0];
    
    this.configStore.startLoadingPanel();
    await this.getUserDetails();
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
    this.userImage = this.userDetails?.image != null ? this.userDetails?.image : "";
    console.log(this.userDetails);
  }

  editProfile(){
    this.editMode = !this.editMode;
  }

  async saveProfile(){
    if(this.userDetails.firstName.length < 1){
      this.alertService.showError("Imię nie może być puste");
    }else if(this.userDetails.lastName.length < 1){
      this.alertService.showError("Nazwisko nie może być puste");
    } else{
      let patchOperations = [];

      patchOperations.push(new Operation(OperationEnum.REPLACE, "firstName", this.userDetails.firstName));
      patchOperations.push(new Operation(OperationEnum.REPLACE, "lastName", this.userDetails.lastName));
      patchOperations.push(new Operation(OperationEnum.REPLACE, "description", this.userDetails.description));
      patchOperations.push(new Operation(OperationEnum.REPLACE, "birthDate", this.userDetails.dateOfBirth));
      
      this.configStore.startLoadingPanel();
      this.userDetails = await this.userService.patchCurrentUser(patchOperations).toPromise();
      this.configStore.stopLoadingPanel();

      this.editMode = false;
      this.alertService.showSuccess("Zaktualizowano");
    }
  }
}
