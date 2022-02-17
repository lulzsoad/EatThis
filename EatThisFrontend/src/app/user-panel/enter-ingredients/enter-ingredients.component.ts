import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ConfigStore } from 'src/app/app-config/config-store';
import { IngredientPosition } from 'src/app/models/app-models/ingredient-position.model';
import { IngredientCategory } from 'src/app/models/ingredient-category.model';
import { Ingredient } from 'src/app/models/ingredient.model';
import { IngredientCategoryService } from 'src/app/services/ingredient-category.service';
import { IngredientService } from 'src/app/services/ingredient.service';
import { RecipeService } from 'src/app/services/recipe.service';

@Component({
  selector: 'app-enter-ingredients',
  templateUrl: './enter-ingredients.component.html',
  styleUrls: ['./enter-ingredients.component.scss']
})
export class EnterIngredientsComponent implements OnInit {
  public ingredients: Ingredient[];
  public ingredientCategories: IngredientCategory[];
  public ingredientPositions: IngredientPosition[] = [];
  public selectedIngredients: IngredientPosition[] = [];
  public filter: string;

  private ingredientPositionsCopy = this.ingredientPositions;
  private skip: number = 0;
  private take: number = 5;

  constructor(
    private configStore: ConfigStore,
    private ingredientService: IngredientService,
    private ingredientCategoryService: IngredientCategoryService,
    private recipeService: RecipeService,
    private router: Router,
    private route: ActivatedRoute
  ) { }

  async ngOnInit(): Promise<void> {
    this.configStore.startLoadingPanel();
    await Promise.all([this.getIngredients(), this.getIngredientCategories()]) ;
    this.initializeData();
    this.configStore.stopLoadingPanel();
  }

  async getIngredients(){
    this.ingredients = await this.ingredientService.getAll().toPromise();
  }

  async getIngredientCategories(){
    this.ingredientCategories = await this.ingredientCategoryService.getAll().toPromise();
  }

  filterIngredientsByCategoryId(ingredientCategoryId: number){
    this.manageVisibility(ingredientCategoryId)
    return this.ingredientPositions.filter(x => x.ingredient.ingredientCategory.id == ingredientCategoryId);
  }

  initializeData(){
    for(let ingredient of this.ingredients){
      this.ingredientPositions.push(new IngredientPosition(ingredient));
    }
  }

  selectIngredient(ingredientPosition: IngredientPosition){
    ingredientPosition.isSelected = !ingredientPosition.isSelected;
    if(ingredientPosition.isSelected){
      this.selectedIngredients.push(ingredientPosition);
    } else{
      this.selectedIngredients
        .splice(this.selectedIngredients
          .indexOf(this.selectedIngredients
            .filter(x => x.ingredient == ingredientPosition.ingredient)[0]), 1)
    }
    
  }

  filterIngredients(event: KeyboardEvent){
    this.ingredientPositions = this.ingredientPositionsCopy
      .filter(x => x.ingredient.name.toLowerCase().includes(this.filter.toLowerCase()));
  }

  manageVisibility(ingredientCategoryId: number){
    let categoryElement = document.getElementById(ingredientCategoryId.toString());
    if(this.ingredientPositions.filter(x => x.ingredient.ingredientCategory.id == ingredientCategoryId).length < 1){
      if(categoryElement){
        categoryElement.style.display = "none";
      }
    }else{
      if(categoryElement){
        categoryElement.style.display = "block";
      }
    }
  }

  getSelectedIngredients(){
    return this.ingredientPositions.filter(x => x.isSelected);
  }

  async submit(){
    this.configStore.startLoadingPanel();
    let ingredients: Ingredient[] = [];
    for(let ingredientPosition of this.selectedIngredients){
      ingredients.push(ingredientPosition.ingredient);
    }

    for(let i = 0; i < ingredients.length; i++){
      delete ingredients[i].ingredientCategory;
    }

    let ingredientsJson = encodeURIComponent(JSON.stringify(ingredients));
    this.configStore.stopLoadingPanel();
    this.router.navigate(['../recipes/by-ingredients'], {relativeTo: this.route, queryParams: {ingredients: ingredientsJson}})
  }
}
