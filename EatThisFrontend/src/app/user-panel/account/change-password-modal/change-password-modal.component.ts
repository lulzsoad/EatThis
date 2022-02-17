import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { DialogContentBase, DialogRef } from '@progress/kendo-angular-dialog';

@Component({
  selector: 'app-change-password-modal',
  templateUrl: './change-password-modal.component.html',
  styleUrls: ['./change-password-modal.component.scss']
})
export class ChangePasswordModalComponent extends DialogContentBase {
  @Input() public oldPassword: string = "";
  @Input() public newPassword: string = "";
  @Input() public confirmPassword: string = "";

  public formGroup: FormGroup = this.fb.group({
    oldPassword: [this.oldPassword, Validators.required],
    newPassword: [this.newPassword, Validators.required],
    confirmPassword: [this.confirmPassword, Validators.required],
  });

  constructor(public dialog: DialogRef, private fb: FormBuilder) {
    super(dialog);
  }

}
