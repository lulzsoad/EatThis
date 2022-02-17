import { Component, OnInit} from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { DialogCloseResult, DialogService } from '@progress/kendo-angular-dialog';
import { ConfigStore } from 'src/app/app-config/config-store';
import { RoleEnum } from 'src/app/enums/role-enum.enum';
import { ChangePassword } from 'src/app/models/app-models/change-password.model';
import { DataChunk } from 'src/app/models/app-models/data-chunk.model';
import { Operation, OperationEnum} from 'src/app/models/app-models/patch-operations.model';
import { Bookmark } from 'src/app/models/bookmark.model';
import { Recipe } from 'src/app/models/recipe.model';
import { Report } from 'src/app/models/report.model';
import { UserDetails } from 'src/app/models/user-details.model';
import { AccountService } from 'src/app/services/account.service';
import { AlertService } from 'src/app/services/app-services/alert.service';
import { DateFormat } from 'src/app/services/app-services/date.service';
import { FileService } from 'src/app/services/app-services/file.service';
import { RecipeService } from 'src/app/services/recipe.service';
import { ReportService } from 'src/app/services/report.service';
import { UserService } from 'src/app/services/user.service';
import { ChangePasswordModalComponent } from '../change-password-modal/change-password-modal.component';

@Component({
  selector: 'app-my-account',
  templateUrl: './my-account.component.html',
  styleUrls: ['./my-account.component.scss']
})
export class MyAccountComponent implements OnInit {
  public bookmarks: Bookmark[] = [];
  public myRecipes: Recipe[];
  public myReports: Report[];
  public selectedBookmark: Bookmark;
  public userDetails: UserDetails;
  public userImage: string = "";
  public editMode: boolean = false;
  public DateFormat = DateFormat;
  public isImageUploading = false;

  public role = RoleEnum;

  constructor(
    private userService: UserService,
    private configStore: ConfigStore,
    private alertService: AlertService,
    private recipeService: RecipeService,
    private reportService: ReportService,
    private dialogService: DialogService,
    private accountService: AccountService,
    private router: Router,
    private route: ActivatedRoute
    ) { }

  async ngOnInit(): Promise<void> {
    this.initializeBookmarks();
    this.selectedBookmark = this.bookmarks.filter(x => x.isActive)[0];
    
    this.configStore.startLoadingPanel();
    await Promise.all([this.getUserDetails(), this.getMyRecipes(), this.getMyReports()]);
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
  }

  editProfile(){
    this.editMode = !this.editMode;
  }

  async saveProfile(){
    if(this.userDetails.firstName.length < 1){
      this.alertService.showError("Imię nie może być puste");
    }else if(this.userDetails.lastName.length < 1){
      this.alertService.showError("Nazwisko nie może być puste");
    } else {
      let patchOperations = [
        new Operation(OperationEnum.REPLACE, "firstName", this.userDetails.firstName),
        new Operation(OperationEnum.REPLACE, "lastName", this.userDetails.lastName),
        new Operation(OperationEnum.REPLACE, "description", this.userDetails.description),
        new Operation(OperationEnum.REPLACE, "birthDate", this.userDetails.dateOfBirth)
      ];
      
      this.configStore.startLoadingPanel();
      this.userDetails = await this.userService.patchCurrentUser(patchOperations).toPromise();
      this.configStore.stopLoadingPanel();

      this.editMode = false;
      this.alertService.showSuccess("Zaktualizowano");
    }
  }

  async uploadImage(event){
    let image = event.target.files[0] as File;
    let extension = `.${image.type.split('/')[1]}`;

    if (image.size > this.configStore.getImageUploadFileRestriction().maxFileSize){
      this.alertService.showError("Zbyt duży rozmiar pliku");
      return;
    } else if (!this.configStore.getImageUploadFileRestriction().allowedExtensions.includes(extension)){
      this.alertService.showError("Niedozwolony format pliku");
    } else {
      this.configStore.startLoadingPanel();
      this.isImageUploading = true;

      this.userDetails.image = await FileService.ConvertToBase64(image) as string;
      let patchOperations = [
        new Operation(OperationEnum.REPLACE, "image", this.userDetails.image)
      ];

      this.userDetails = await this.userService.patchCurrentUser(patchOperations).toPromise();

      this.isImageUploading = false;
      this.configStore.stopLoadingPanel();

      this.alertService.showSuccess("Zaktualizowano zdjęcie profilowe");
    }
  }

  async getMyRecipes(){
    this.myRecipes = await this.recipeService.getCurrentUserRecipes().toPromise();
  }

  navigateToRecipe(recipeId: number){
    this.router.navigate(['../../recipes', recipeId], {relativeTo: this.route});
  }

  async getMyReports(){
    this.myReports = await this.reportService.getCurrentUserReports().toPromise();
    console.log(this.myReports);
  }

  changePasswordClick(){
    const dialogRef = this.dialogService.open({
      content: ChangePasswordModalComponent,
      title: "Zmień hasło",
      actions: [{ text: "Anuluj" }, { text: "Zmień hasło", themeColor: "primary" }],
      preventAction: (ev: any, dialog): boolean => {
        const formGroup = dialog.content.instance.formGroup;
        
        if(ev.text == "Anuluj"){
          dialog.close();
          return true;
        }

        if (!formGroup.valid) {
          return !formGroup.valid;
        }

        if(formGroup.value.newPassword != formGroup.value.confirmPassword){
          this.alertService.showError("Hasła nie zgadzają się ze sobą");
          return false;
        }

        if(formGroup.value.newPassword.length < 6){
          this.alertService.showError("Hasła nie zgadzają się ze sobą");
          return false;
        }
        
        let isOk = this.changePassword(new ChangePassword(formGroup.value.oldPassword, formGroup.value.newPassword));

        if(isOk){
          dialog.close();
        }

        return true;
      },
    });
  }

  async changePassword(changePasswordVm: ChangePassword){
    this.configStore.startLoadingPanel();
        await this.accountService
          .changePasswordModal(changePasswordVm).toPromise();
        this.configStore.stopLoadingPanel();
        this.alertService.showInfo("Zmieniono hasło");
        return true;
  }
}
