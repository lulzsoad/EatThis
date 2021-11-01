import { Component, Input, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { IngredientCategory } from 'src/app/models/ingredient-category.model';
import { IngredientCategoryService } from 'src/app/services/ingredient-category.service';
import { WindowService } from 'src/app/services/window.service';

@Component({
  selector: 'app-ingredient-category-form-modal',
  templateUrl: './ingredient-category-form-modal.component.html',
  styleUrls: ['./ingredient-category-form-modal.component.scss']
})
export class IngredientCategoryFormModalComponent implements OnInit {
  @Input() public opened: boolean;
  @Input() public isNew: boolean;
  @Input() public ingredientCategory: IngredientCategory;
  public form: FormGroup = new FormGroup({
    id: new FormControl(),
    name: new FormControl()
  });

  constructor(private ingredientCategoryService: IngredientCategoryService, private windowService: WindowService) { }

  ngOnInit(): void {

  }

  public close() {
    this.opened = false;
    this.windowService.closeWindow.next(false);
  }

  public open() {
    this.opened = true;
  }

  public async submit() {
    this.ingredientCategory = this.form.value;
    await this.ingredientCategoryService.createIngredientCategoryModalSaved.next(this.ingredientCategory);
    this.close();
  }

}
