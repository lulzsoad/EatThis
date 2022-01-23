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
import { AlertService } from 'src/app/services/app-services/alert.service';
import { RecipeDifficulties } from 'src/app/models/app-models/difficulty.modetl';
import { FileService } from 'src/app/services/app-services/file.service';
import { SelectEvent } from '@progress/kendo-angular-upload';
import { RecipeService } from 'src/app/services/recipe.service';
import { ActivatedRoute, Router } from '@angular/router';

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
  public stepImages = [];
  public recipe: Recipe = new Recipe();
  public recipeImage = [];
  public uploadSaveUrl = "";
  public uploadRemoveUrl = "";
  public uploadFileRestrictions;
  public difficulties: string[] = RecipeDifficulties.difficuilties;

  private isValidationOk: boolean = true;
  constructor(
    private categorySerice: CategoryService,
    private ingredientService: IngredientService,
    private unitService: UnitService,
    private configStore: ConfigStore,
    private alertService: AlertService,
    private recipeService: RecipeService,
    private router: Router,
    private route: ActivatedRoute
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
      steps: new FormControl([]),
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
    this.ingredients = await this.ingredientService.getAll().toPromise();
    this.groupedIngredients = groupBy(this.ingredients, [{field: "ingredientCategory.name"},]);
  }

  async getCategories(){
    this.categories = await this.categorySerice.getAll().toPromise();
  }

  async getUnits(){
    this.units = await this.unitService.getAll().toPromise();
  }

  ingredientsValueChange(value: Ingredient[]){
    console.log(value);
    for(let i = 0; i < value.length; i++){
      if(!this.ingredientsQuantity[i]){
        this.ingredientsQuantity[i] = new IngredientQuantity();
        this.ingredientsQuantity[i].description = "";
        this.ingredientsQuantity[i].quantity = 0;
        this.ingredientsQuantity[i].unit = this.units[0];
        this.ingredientsQuantity[i].ingredient = value[i];
      }
    }
  }

  addStep(){
    let step = new Step();
    step.description = "";
    step.image = "";
    step.order = 0;
    this.steps.push(step);
    this.stepImages.push({})
  }

  removeStep(index: number){
    this.steps.splice(index, 1);
  }

  async addRecipe(){
    this.form.markAllAsTouched();
    
    this.recipe = this.form.value;
    this.validate();
    
    if(this.isValidationOk){
      this.configStore.startLoadingPanel();

      this.recipe.image = this.recipeImage[0] != null ? await FileService.ConvertToBase64(this.recipeImage[0]) as string : "";
      this.recipe.ingredientQuantities = this.ingredientsQuantity;
      this.recipe.isVisible= true;
      for(let i = 0; i < this.steps.length; i++){
        this.steps[i].order = i;
      }
      this.recipe.steps = this.steps;
      delete this.recipe.id;
      console.log(this.recipe);

      await this.recipeService.addRecipe(this.recipe).toPromise();

      this.configStore.stopLoadingPanel();
      this.alertService.showSuccess("Dodano przepis");
      this.router.navigate(['admin-panel']);
    } 
  }

  validate(){
    if(this.ingredientsQuantity.length < 1){
      this.alertService.showError("Nie podano składników");
      this.isValidationOk = false;
      return;
    }else if(this.steps.length < 1){
      this.alertService.showError("Nie podano kroków");
      this.isValidationOk = false;
      return;
    }else if(this.recipe.category == null){
      this.alertService.showError("Nie wybrano kategorii");
      this.isValidationOk = false;
      return;
    }else if(this.recipe.name == null || this.recipe.name.length < 1){
      this.alertService.showError("Nie podano nazwy");
      this.isValidationOk = false;
      return;
    }else if(this.recipeImage == null || this.recipeImage.length < 1){
      this.alertService.showError("Nie załadowano obrazu");
      this.isValidationOk = false;
      return;
    } else if(this.recipe.difficulty == null || this.recipe.difficulty.length < 1){
      this.alertService.showError("Nie wybrano trudności");
      this.isValidationOk = false;
      return;
    }
    for(let x of this.steps){
      if(x.description == null || x.description.length < 1){
        this.alertService.showError("Nie uzupełniono opisu kroku");
        this.isValidationOk = false;
        return;
      }
    }
    for(let x of this.ingredientsQuantity){
      if(x.quantity == null || x.quantity == 0){
        this.alertService.showError("Nie podano wszystkich ilości składników");
        return;
      }
    }

    this.isValidationOk = true;
  }

  async onSelectStepImageFile(event: SelectEvent, index: number){
    this.steps[index].image = await FileService.ConvertToBase64(event.files[0].rawFile) as string;
  }
}
