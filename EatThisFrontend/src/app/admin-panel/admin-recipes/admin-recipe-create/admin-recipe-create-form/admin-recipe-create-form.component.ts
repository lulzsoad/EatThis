import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';

@Component({
  selector: 'app-admin-recipe-create-form',
  templateUrl: './admin-recipe-create-form.component.html',
  styleUrls: ['./admin-recipe-create-form.component.scss']
})
export class AdminRecipeCreateFormComponent implements OnInit {
  public form: FormGroup
  constructor() { }

  ngOnInit(): void {
    this.form = new FormGroup({
      id: new FormControl(),
      name: new FormControl(),
      subName: new FormControl(),
      description: new FormControl(),
      difficulty: new FormControl(),
      isVisivble: new FormControl(),
      ingredientQuantities: new FormControl(),
      steps: new FormControl(),
      category: new FormControl(),
      image: new FormControl(),
      time: new FormControl(),
      personQuantity: new FormControl(),
    })
  }
}
