import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { ConfigStore } from 'src/app/app-config/config-store';
import { RecipeDifficulties } from 'src/app/models/app-models/difficulty.modetl';
import { IngredientPosition } from 'src/app/models/app-models/ingredient-position.model';
import { Category } from 'src/app/models/category.model';
import { IngredientCategory } from 'src/app/models/ingredient-category.model';
import { IngredientQuantity } from 'src/app/models/ingredient-quantity.model';
import { Ingredient } from 'src/app/models/ingredient.model';
import { Step } from 'src/app/models/step.model';
import { Unit } from 'src/app/models/unit.model';
import { CategoryService } from 'src/app/services/category.service';
import { IngredientCategoryService } from 'src/app/services/ingredient-category.service';
import { IngredientService } from 'src/app/services/ingredient.service';
import { UnitService } from 'src/app/services/unit.service';
import { WindowService } from 'src/app/services/window.service';

@Component({
  selector: 'app-recipe-add',
  templateUrl: './recipe-add.component.html',
  styleUrls: ['./recipe-add.component.scss']
})
export class RecipeAddComponent implements OnInit, OnDestroy {
  public categories: Category[];
  public ingredients: Ingredient[];
  public ingredientPositions: IngredientPosition[] = [];
  public ingredientCategories: IngredientCategory[];
  public units: Unit[];
  public ingredientQuantities: IngredientQuantity[] = [];
  public steps: Step[] = [];
  public difficulties = RecipeDifficulties.difficuilties;
  public modalOpened: boolean = false;

  private modalOpenedSubscription: Subscription;
  private addIngredientSubscription: Subscription;

  constructor(
    private configStore: ConfigStore,
    private categoryService: CategoryService,
    private ingredientService: IngredientService,
    private ingredientCategoryService: IngredientCategoryService,
    private unitService: UnitService,
    private windowService: WindowService
    ) { }


  async ngOnInit(): Promise<void> {
    this.initializeSubscriptions();
    this.configStore.startLoadingPanel();
    await Promise.all([this.getCategories(), this.getIngredients(), this.getIngredientCategories(), this.getUnits()]);
    this.initializeData();
    this.configStore.stopLoadingPanel();
  }

  ngOnDestroy(): void {
    this.disposeSubscriptions();
  }

  initializeData(){
    for(let ingredient of this.ingredients){
      this.ingredientPositions.push(new IngredientPosition(ingredient));
    }
  }

  async getCategories(){
    this.categories = await this.categoryService.getAll().toPromise();
  }

  async getIngredients(){
    this.ingredients = await this.ingredientService.getAll().toPromise();
  }

  async getIngredientCategories(){
    this.ingredientCategories = await this.ingredientCategoryService.getAll();
  }

  async getUnits(){
    this.units = await this.unitService.getAll().toPromise();
  }

  initializeSubscriptions(){
    this.modalOpenedSubscription = this.windowService.openWindow.subscribe(isOpened => {
      this.modalOpened = isOpened;
    });

    this.addIngredientSubscription = this.ingredientService.addIngredient.subscribe((ingredientPosition: IngredientPosition) =>{
      let ingredientQuantity = new IngredientQuantity();
      ingredientQuantity.ingredient = ingredientPosition.ingredient;

      if(ingredientPosition.isSelected){
        this.ingredientQuantities.push(ingredientQuantity);
      } else{
        this.ingredientQuantities.splice(this.ingredientQuantities.indexOf(ingredientQuantity), 1);
      }
      
    })
  }

  disposeSubscriptions(){
    this.modalOpenedSubscription.unsubscribe();
  }

  openModal(){
    this.windowService.openWindow.next(true);
  }

  addStep(){
    let step: Step = new Step();
    this.steps.push(step);
  }

  removeStep(index){
    this.steps.splice(index, 1);
  }
}
