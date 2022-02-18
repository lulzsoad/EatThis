import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { DialogContentBase, DialogRef } from '@progress/kendo-angular-dialog';
import { Ingredient } from 'src/app/models/ingredient.model';
import { ProposedIngredient } from 'src/app/models/proposed-recipe/proposed-ingredient.model';

@Component({
  selector: 'app-change-ingredient-modal',
  templateUrl: './change-ingredient-modal.component.html',
  styleUrls: ['./change-ingredient-modal.component.scss']
})
export class ChangeIngredientModalComponent extends DialogContentBase {
  @Input() proposedIngredient: ProposedIngredient = new ProposedIngredient;
  @Input() changedIngredient: Ingredient = new Ingredient;
  @Input() ingredients: Ingredient[] = [];

  constructor(public dialog: DialogRef, private fb: FormBuilder) {
    super(dialog);
  }

}
