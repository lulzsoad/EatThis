import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ConfigStore } from 'src/app/app-config/config-store';
import { DataChunk } from 'src/app/models/app-models/data-chunk.model';
import { Bookmark } from 'src/app/models/bookmark.model';
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
  public page: number = 1;
  public pages: Bookmark[] = [];
  public selectedPage: Bookmark;
  public count: number;

  private selectedIngredients: Ingredient[];
  private ingredientsUrl: string;
  private skip: number = 0;
  private take: number = 5;

  constructor(
    private configStore: ConfigStore,
    private recipeService: RecipeService,
    private router: Router,
    private route: ActivatedRoute
  ) { }

  async ngOnInit(): Promise<void> {
    this.route.queryParamMap.subscribe(async () => {
      this.configStore.startLoadingPanel();
      this.ingredientsUrl = this.route.snapshot.queryParamMap.get('ingredients');
      this.page = +this.route.snapshot.queryParamMap.get('page');
      await this.getRecipesByIngredients();
      this.getSelectedIngredients();
      this.getIncludedIngredients();
      this.getMissingIngredients();
      this.loadPages();
      this.configStore.stopLoadingPanel();
    });
  }

  async getRecipesByIngredients(){
    this.recipes = await this.recipeService.getRecipesByIngredients(this.ingredientsUrl, (this.page - 1) * this.take, this.take).toPromise();
    this.count = this.recipes.total;
  }

  getSelectedIngredients(){
    this.selectedIngredients = [];
    let decodedIngredients = decodeURIComponent(this.ingredientsUrl).split(',')
    for(var item of decodedIngredients){
      let ingredient = new Ingredient(item.split('_')[1]);
      ingredient.id = +item.split('_')[0];
      delete ingredient.ingredientCategory;
      this.selectedIngredients.push(ingredient);
    };
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

  loadPages(){
    this.pages = [];
    let pagesCount = Math.ceil(this.count / this.take);
        for(let i = 0; i < pagesCount; i++){
          this.pages.push(new Bookmark(i.toString(), (i+1).toString()));
        }
  }

  async changePage(page: Bookmark){
    this.router.navigate([], {queryParams: {ingredients: this.ingredientsUrl, page: page.label}})
  }
}
