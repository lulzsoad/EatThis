import { Component, Input, OnInit } from '@angular/core';
import { IngredientPosition } from 'src/app/models/app-models/ingredient-position.model';
import { Window } from 'src/app/models/app-models/window.model';
import { IngredientCategory } from 'src/app/models/ingredient-category.model';
import { Ingredient } from 'src/app/models/ingredient.model';
import { IngredientService } from 'src/app/services/ingredient.service';
import { WindowService } from 'src/app/services/window.service';

@Component({
  selector: 'app-ingredients-modal',
  templateUrl: './ingredients-modal.component.html',
  styleUrls: ['./ingredients-modal.component.scss']
})
export class IngredientsModalComponent implements OnInit {
  @Input() public ingredientPositions: IngredientPosition[] = [];
  @Input() public ingredientCategories: IngredientCategory[];
  public ingredientsCopy: IngredientPosition[];
  public filter: string;

  private windowId = "addIngredients";

  constructor(
    private windowService: WindowService,
    private ingredientService: IngredientService
  ) { }

  ngOnInit(): void {
    this.ingredientsCopy = [...this.ingredientPositions];
  }

  close(){
    this.windowService.openWindow.next(false);
  }

  filterIngredientsByCategory(ingredientCategory: IngredientCategory): IngredientPosition[]{
    return this.ingredientsCopy?.filter(x => x.ingredient.ingredientCategory.id == ingredientCategory.id);
  }

  filterValueChange(){
    this.ingredientsCopy = this.ingredientPositions.filter(x => x.ingredient.name.toLowerCase().includes(this.filter.toLowerCase()));
  }

  selectIngredient(ingredientPosition: IngredientPosition){
    ingredientPosition.isSelected = !ingredientPosition.isSelected;
    this.ingredientService.addIngredientModal.next(ingredientPosition);
  }
}
