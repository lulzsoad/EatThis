import { Component, Input, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { ConfigStore } from 'src/app/app-config/config-store';
import { IngredientCategory } from 'src/app/models/ingredient-category.model';
import { Ingredient } from 'src/app/models/ingredient.model';
import { IngredientCategoryService } from 'src/app/services/ingredient-category.service';
import { IngredientService } from 'src/app/services/ingredient.service';
import { WindowService } from 'src/app/services/window.service';

@Component({
  selector: 'app-ingredient-create-modal',
  templateUrl: './ingredient-create-modal.component.html',
  styleUrls: ['./ingredient-create-modal.component.scss']
})
export class IngredientCreateModalComponent implements OnInit {
  @Input() public opened: boolean;
  @Input() public isNew: boolean;
  @Input() public ingredient: Ingredient;
  @Input() public selectedValue;
  @Input() public ingredientCategories: IngredientCategory[]; 
  public form: FormGroup = new FormGroup({
    id: new FormControl(),
    name: new FormControl(),
    ingredientCategory: new FormControl()
  });

  constructor(
    private ingredientService: IngredientService, 
    private windowService: WindowService, 
    ) { }

  async ngOnInit(): Promise<void> {
    
  }

  public close() {
    this.opened = false;
    this.windowService.closeWindow.next(false);
  }

  public open() {
    this.opened = true;
  }

  public async submit() {
    console.log(this.selectedValue);
    this.ingredient = this.form.value;
    await this.ingredientService.createIngredientModalSaved.next(this.ingredient);
    this.close();
  }
}
