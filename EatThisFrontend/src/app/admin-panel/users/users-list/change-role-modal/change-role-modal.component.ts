import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { DialogContentBase, DialogRef } from '@progress/kendo-angular-dialog';
import { Role } from 'src/app/models/role.model';
import { UserDetails } from 'src/app/models/user-details.model';

@Component({
  selector: 'app-change-role-modal',
  templateUrl: './change-role-modal.component.html',
  styleUrls: ['./change-role-modal.component.scss']
})
export class ChangeRoleModalComponent extends DialogContentBase {
  @Input() public user: UserDetails = new UserDetails;
  @Input() public roles: Role[] = [];
  @Input() public newRole: Role = new Role;

  public formGroup: FormGroup = this.fb.group({
    role: [this.newRole]
  });

  constructor(public dialog: DialogRef, private fb: FormBuilder) {
    super(dialog);
  }

}
