import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { Category } from 'src/app/models/category.model';
import { IngredientQuantity } from 'src/app/models/ingredient-quantity.model';
import { Ingredient } from 'src/app/models/ingredient.model';
import { Recipe } from 'src/app/models/recipe.model';
import { Unit } from 'src/app/models/unit.model';
import { CategoryService } from 'src/app/services/category.service';
import { IngredientService } from 'src/app/services/ingredient.service';
import { UnitService } from 'src/app/services/unit.service';

@Component({
  selector: 'app-admin-recipe-create-form',
  templateUrl: './admin-recipe-create-form.component.html',
  styleUrls: ['./admin-recipe-create-form.component.scss']
})
export class AdminRecipeCreateFormComponent implements OnInit {
  public form: FormGroup
  public ingredients: Ingredient[];
  public chosenIngredients: Ingredient[] = [];
  public ingredientsQuantity: IngredientQuantity[];
  public categories: Category[];
  public units: Unit[];
  public loadingPanelVisible = false;
  public recipe: Recipe;
  constructor(
    private categorySerice: CategoryService,
    private ingredientService: IngredientService,
    private unitService: UnitService
  ) { }

  async ngOnInit(): Promise<void> {
    this.loadingPanelVisible = true;

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
    });

    await this.getIngredients();
    await this.getCategories();
    await this.getUnits();

    this.loadingPanelVisible = false;
  }

  async getIngredients(){
    this.ingredients = await this.ingredientService.getAll();
  }

  async getCategories(){
    this.categories = await this.categorySerice.getAll();
  }

  async getUnits(){
    this.units = await this.unitService.getAll();
  }
}
