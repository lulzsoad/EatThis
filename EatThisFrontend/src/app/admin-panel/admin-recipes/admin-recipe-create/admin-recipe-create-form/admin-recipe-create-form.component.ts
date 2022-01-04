import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { groupBy, GroupResult } from '@progress/kendo-data-query';
import { Image } from "src/app/models/image.model";
import { ConfigStore } from 'src/app/app-config/config-store';
import { Category } from 'src/app/models/category.model';
import { IngredientQuantity } from 'src/app/models/ingredient-quantity.model';
import { Ingredient } from 'src/app/models/ingredient.model';
import { Recipe } from 'src/app/models/recipe.model';
import { Step } from 'src/app/models/step.model';
import { Unit } from 'src/app/models/unit.model';
import { CategoryService } from 'src/app/services/category.service';
import { IngredientService } from 'src/app/services/ingredient.service';
import { UnitService } from 'src/app/services/unit.service';
import { AlertService } from 'src/app/services/alert.service';

@Component({
  selector: 'app-admin-recipe-create-form',
  templateUrl: './admin-recipe-create-form.component.html',
  styleUrls: ['./admin-recipe-create-form.component.scss']
})
export class AdminRecipeCreateFormComponent implements OnInit {
  public form: FormGroup
  public ingredients: any[];
  public groupedIngredients: GroupResult[];
  public chosenIngredients: Ingredient[] = [];
  public ingredientsQuantity: IngredientQuantity[] = [];
  public categories: Category[];
  public units: Unit[];
  public steps: Step[] = [];
  public stepImages: Array<any> = [];
  public recipe: Recipe = new Recipe();
  public uploadSaveUrl = "";
  public uploadRemoveUrl = "";
  public uploadFileRestrictions;
  constructor(
    private categorySerice: CategoryService,
    private ingredientService: IngredientService,
    private unitService: UnitService,
    private configStore: ConfigStore,
    private alertService: AlertService
  ) { }

  async ngOnInit(): Promise<void> {
    this.configStore.startLoadingPanel();

    this.form = new FormGroup({
      id: new FormControl(),
      name: new FormControl(),
      subName: new FormControl(),
      description: new FormControl(),
      difficulty: new FormControl(),
      isVisivble: new FormControl(),
      ingredientQuantities: new FormControl(),
      steps: new FormControl(),
      stepImages: new FormControl([]),
      category: new FormControl(),
      image: new FormControl(),
      time: new FormControl(),
      personQuantity: new FormControl(),
    });

    await Promise.all([this.getIngredients(), this.getCategories(), this.getUnits()]);
    this.uploadFileRestrictions = this.configStore.getImageUploadFileRestriction();
    this.configStore.stopLoadingPanel();
  }

  async getIngredients(){
    this.ingredients = await this.ingredientService.getAll();
    this.groupedIngredients = groupBy(this.ingredients, [{field: "ingredientCategory.name"},]);
  }

  async getCategories(){
    this.categories = await this.categorySerice.getAll();
  }

  async getUnits(){
    this.units = await this.unitService.getAll();
  }

  ingredientsValueChange(value: any){
    for(let i = 0; i < value.length; i++){
      if(!this.ingredientsQuantity[i]){
        this.ingredientsQuantity[i] = new IngredientQuantity();
        this.ingredientsQuantity[i].description = "";
        this.ingredientsQuantity[i].quantity = 0;
        this.ingredientsQuantity[i].unit = this.units[0];
      }
    }
  }

  addStep(){
    this.steps.push(new Step());
    this.stepImages.push({})
  }

  addRecipe(){
    this.form.markAllAsTouched();
    
    this.recipe = this.form.value;
    for(let step in this.steps){
      this.recipe.steps = [];
      this.recipe.steps.push()
    }
    console.log(this.recipe);
    console.log(this.stepImages);
    if(this.ingredientsQuantity.length < 1){
      this.alertService.showError("Nie podano składników");
      return;
    }
    if(this.steps.length < 1){
      this.alertService.showError("Nie podano kroków");
      return;
    }

    
  }
}
