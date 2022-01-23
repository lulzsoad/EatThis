import { Component, Input, OnInit } from '@angular/core';
import { IngredientPosition } from 'src/app/models/app-models/ingredient-position.model';
import { IngredientCategory } from 'src/app/models/ingredient-category.model';
import { Ingredient } from 'src/app/models/ingredient.model';
import { AlertService } from 'src/app/services/app-services/alert.service';
import { IngredientService } from 'src/app/services/ingredient.service';
import { WindowService } from 'src/app/services/window.service';

@Component({
  selector: 'app-propose-ingredient-modal',
  templateUrl: './propose-ingredient-modal.component.html',
  styleUrls: ['./propose-ingredient-modal.component.scss']
})
export class ProposeIngredientModalComponent implements OnInit {
  @Input() ingredientCategories: IngredientCategory[];
  @Input() ingredientPosition: IngredientPosition;
  public selectedCategory: IngredientCategory;

  oldIngredient: Ingredient;

  constructor(
    private windowSerice: WindowService, 
    private ingredientService: IngredientService,
    private alertService: AlertService
    ) { }

  ngOnInit(): void {
    this.initializeData();
  }

  initializeData(){
    if(!this.ingredientPosition.isNew){
      this.oldIngredient = new Ingredient();
      this.oldIngredient.name = this.ingredientPosition.ingredient.name;
      this.oldIngredient.ingredientCategory = new IngredientCategory();
      this.oldIngredient.ingredientCategory = this.ingredientPosition.ingredient.ingredientCategory;
      this.oldIngredient.ingredientCategory.id = this.ingredientPosition.ingredient.ingredientCategory.id;
      this.oldIngredient.ingredientCategory.name = this.ingredientPosition.ingredient.ingredientCategory.name;
    }
    if(!this.selectedCategory && !this.ingredientPosition.ingredient.ingredientCategory){
      this.selectedCategory = this.ingredientCategories[0];
    } else{
      this.selectedCategory = this.ingredientPosition.ingredient.ingredientCategory;
    }
  }

  close(){
    if(!this.ingredientPosition.isNew){
      console.log(this.oldIngredient);
      this.ingredientPosition.ingredient.name = this.oldIngredient.name;
      this.ingredientPosition.ingredient.ingredientCategory = this.oldIngredient.ingredientCategory;
    }
    this.windowSerice.proposeIngredientWindowOpen.next(false);
  }

  submit(){
    if(!this.validate())
      return;
    
    this.ingredientPosition.isProposed = true;
    this.ingredientPosition.ingredient.ingredientCategory = this.selectedCategory;
    console.log(this.ingredientPosition);

    if(this.ingredientPosition.isNew){
      this.ingredientService.proposeIngredientModal.next(this.ingredientPosition);
    }
    this.close();
  }

  validate(): boolean{
    if(!this.ingredientPosition.ingredient.name || this.ingredientPosition.ingredient?.name?.length < 2){
      this.alertService.showError("Nazwa musi zawierać conajmniej 2 znaki");
      return false;
    } else if(!this.selectedCategory){
      this.alertService.showError("Nie wybrano kategorii");
      return false;
    }
    return true;
  }
}
