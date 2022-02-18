import { Component, Input, OnInit } from '@angular/core';
import { DialogContentBase, DialogRef } from '@progress/kendo-angular-dialog';

@Component({
  selector: 'app-discard-proposed-recipe-reason-modal',
  templateUrl: './discard-proposed-recipe-reason-modal.component.html',
  styleUrls: ['./discard-proposed-recipe-reason-modal.component.scss']
})
export class DiscardProposedRecipeReasonModalComponent extends DialogContentBase {
  @Input() public message: string = "";

  constructor(public dialog: DialogRef) { 
    super(dialog);
  }

}
