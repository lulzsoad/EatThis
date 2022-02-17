import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ConfigStore } from 'src/app/app-config/config-store';
import { DataChunk } from 'src/app/models/app-models/data-chunk.model';
import { Ingredient } from 'src/app/models/ingredient.model';
import { RecipeByIngredient } from 'src/app/models/recipe-by-ingredient.model';
import { RecipeService } from 'src/app/services/recipe.service';

@Component({
  selector: 'app-recipes-by-ingredients',
  templateUrl: './recipes-by-ingredients.component.html',
  styleUrls: ['./recipes-by-ingredients.component.scss']
})
export class RecipesByIngredientsComponent implements OnInit {
  public recipes: DataChunk<RecipeByIngredient>;

  private selectedIngredients: Ingredient[];
  private ingredientsUrl: string;
  private page: number;
  private skip: number = 0;
  private take: number = 5;

  constructor(
    private configStore: ConfigStore,
    private recipeService: RecipeService,
    private router: Router,
    private route: ActivatedRoute
  ) { }

  async ngOnInit(): Promise<void> {
    this.route.queryParamMap.subscribe(() => {
      this.ingredientsUrl = this.route.snapshot.queryParamMap.get('ingredients');
      this.page = +this.route.snapshot.queryParamMap.get('page');
    });

    this.configStore.startLoadingPanel();
    await this.getRecipesByIngredients();
    this.getSelectedIngredients();
    this.getIncludedIngredients();
    this.getMissingIngredients();
    this.configStore.stopLoadingPanel();

    console.log(this.recipes);
  }

  async getRecipesByIngredients(){
    this.recipes = await this.recipeService.getRecipesByIngredients(this.ingredientsUrl, this.skip, this.take).toPromise();
  }

  getSelectedIngredients(){
    this.selectedIngredients = JSON.parse(decodeURIComponent(this.ingredientsUrl));
  }

  getIncludedIngredients(){
    for(let i = 0; i < this.recipes.data.length; i++){
      this.recipes.data[i].includedIngredients = [];
      for(let j = 0; j < this.recipes.data[i].recipe.ingredientQuantities.length; j++){
        delete this.recipes.data[i].recipe.ingredientQuantities[j].ingredient.ingredientCategory;
        if(this.selectedIngredients.some(elem => {
          return JSON.stringify(this.recipes.data[i].recipe.ingredientQuantities[j].ingredient) === JSON.stringify(elem)
        })){
          this.recipes.data[i].includedIngredients.push(this.recipes.data[i].recipe.ingredientQuantities[j]);
        }
      }
    }
    console.log(this.selectedIngredients);
    console.log(this.recipes);
  }

  getMissingIngredients(){
    for(let i = 0; i < this.recipes.data.length; i++){
      this.recipes.data[i].missingIngredients = [];
      for(let j = 0; j < this.recipes.data[i].recipe.ingredientQuantities.length; j++){
        if(!this.selectedIngredients.some(elem => {
          return JSON.stringify(this.recipes.data[i].recipe.ingredientQuantities[j].ingredient) === JSON.stringify(elem)
        })){
          this.recipes.data[i].missingIngredients.push(this.recipes.data[i].recipe.ingredientQuantities[j]);
        }
      }
    }
  }
  
  navigateToRecipe(recipeId){
    this.router.navigate(['../', recipeId], {relativeTo: this.route})
  }
}
