import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { DialogService } from '@progress/kendo-angular-dialog';
import { PagerSettings } from '@progress/kendo-angular-grid';
import { DataChunk } from 'src/app/models/app-models/data-chunk.model';
import { Role } from 'src/app/models/role.model';
import { UserDetails } from 'src/app/models/user-details.model';
import { AccountService } from 'src/app/services/account.service';
import { AlertService } from 'src/app/services/app-services/alert.service';
import { UserService } from 'src/app/services/user.service';
import { ChangeRoleModalComponent } from './change-role-modal/change-role-modal.component';

@Component({
  selector: 'app-users-list',
  templateUrl: './users-list.component.html',
  styleUrls: ['./users-list.component.scss']
})
export class UsersListComponent implements OnInit {
  public users: DataChunk<UserDetails>;
  public roles: Role[];
  public search: string = "";
  public skip: number = 0;
  public take: number = 20;
  public pageableConfig: PagerSettings = {
    buttonCount: 5,
    type: "numeric",
    info: true,
    previousNext: true,
    position: "bottom"
  }
  public loading: boolean = false;

  constructor(
    private userService: UserService,
    private accountService: AccountService,
    private dialogService: DialogService,
    private alertService: AlertService
    ) { }

  async ngOnInit(): Promise<void> {
    await Promise.all([this.getUsers(), this.getRoles()]);
  }

  private async getUsers(){
    this.loading = true;
    this.users = await this.userService.getChunkOfUsers(this.skip, this.take, this.search).toPromise();
    this.loading = false;
  }

  private async getRoles(){
    this.roles = await this.accountService.getRoles().toPromise();
  }

  async pageChange(event){
    this.take = event.take;
    this.skip = event.skip;
    this.getUsers();
  }

  showDialog(user: UserDetails){
    const dialogRef = this.dialogService.open({
      title: "Zmień rolę",
      content: ChangeRoleModalComponent,
      width: 500,
      actions: [{text: "Anuluj"}, {text: "Zmień", themeColor: "primary"}],
    });

    const userInfo = dialogRef.content.instance;
        userInfo.user = user;
        userInfo.roles = this.roles;
        userInfo.newRole = user.role;

    dialogRef.result.subscribe(async (result: any) => {
      if(result.text == "Zmień"){
        await this.changeRole(user.id, userInfo.newRole);
        this.alertService.showSuccess("Zmieniono");
        dialogRef.close()
        this.loading = true;
        await this.getUsers();
        this.loading = false;
      }
    });
    

  }

  async changeRole(userId, newRole: Role){
    await this.userService.changeUserRole(userId, newRole).toPromise();
  }

  onSearch(){
    this.getUsers();
  }

  onSearchEvent(event){
    if(event.key == "Enter"){
      this.onSearch();
    }
  }
}
