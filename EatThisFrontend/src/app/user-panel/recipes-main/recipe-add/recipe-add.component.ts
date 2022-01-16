import { Component, OnInit } from '@angular/core';
import { ConfigStore } from 'src/app/app-config/config-store';
import { RecipeDifficulties } from 'src/app/models/app-models/difficulty.modetl';
import { Category } from 'src/app/models/category.model';
import { Ingredient } from 'src/app/models/ingredient.model';
import { CategoryService } from 'src/app/services/category.service';
import { IngredientService } from 'src/app/services/ingredient.service';

@Component({
  selector: 'app-recipe-add',
  templateUrl: './recipe-add.component.html',
  styleUrls: ['./recipe-add.component.scss']
})
export class RecipeAddComponent implements OnInit {
  public categories: Category[];
  public ingredients: Ingredient[];
  public difficulties = RecipeDifficulties.difficuilties;

  constructor(
    private configStore: ConfigStore,
    private categoryService: CategoryService,
    private ingredientService: IngredientService
    ) { }

  async ngOnInit(): Promise<void> {
    this.configStore.startLoadingPanel();
    await Promise.all([this.getCategories(), this.getIngredients()]);
    this.configStore.stopLoadingPanel();
  }

  async getCategories(){
    this.categories = await this.categoryService.getAll().toPromise();
  }

  async getIngredients(){
    this.ingredients = await this.ingredientService.getAll().toPromise();
  }
}
