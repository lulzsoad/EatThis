import { Component, OnDestroy, OnInit } from '@angular/core';
import { FileRestrictions, SelectEvent } from '@progress/kendo-angular-upload';
import { stringify } from 'querystring';
import { Subscription } from 'rxjs';
import { ConfigStore } from 'src/app/app-config/config-store';
import { RecipeDifficulties } from 'src/app/models/app-models/difficulty.modetl';
import { IngredientPosition } from 'src/app/models/app-models/ingredient-position.model';
import { Category } from 'src/app/models/category.model';
import { IngredientCategory } from 'src/app/models/ingredient-category.model';
import { IngredientQuantity } from 'src/app/models/ingredient-quantity.model';
import { Ingredient } from 'src/app/models/ingredient.model';
import { ProposedCategory } from 'src/app/models/proposed-recipe/proposed-category.model';
import { ProposedIngredientQuantity } from 'src/app/models/proposed-recipe/proposed-ingredient-quantity.model';
import { ProposedRecipe } from 'src/app/models/proposed-recipe/proposed-recipe.model';
import { Step } from 'src/app/models/step.model';
import { Unit } from 'src/app/models/unit.model';
import { AlertService } from 'src/app/services/app-services/alert.service';
import { FileService } from 'src/app/services/app-services/file.service';
import { CategoryService } from 'src/app/services/category.service';
import { IngredientCategoryService } from 'src/app/services/ingredient-category.service';
import { IngredientService } from 'src/app/services/ingredient.service';
import { RecipeService } from 'src/app/services/recipe.service';
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
  public proposedIngredientQuantities: ProposedIngredientQuantity[] = [];
  public steps: Step[] = [];
  public difficulties = RecipeDifficulties.difficuilties;
  public ingredientPosition: IngredientPosition;
  public proposedCategory: ProposedCategory;
  public proposedRecipe: ProposedRecipe = new ProposedRecipe();
  public selectedCategory: Category;
  public uploadFileRestrictions: FileRestrictions;
  public stepImages = [];
  public recipeImage = {};

  public addIngredientsModalOpened: boolean = false;
  public addProposedIngredientModalOpened: boolean = false;
  public addProposedCategoryModalOpened: boolean = false;


  // Subscriptions
  private modalOpenedSubscription: Subscription;
  private proposedIngredientModalOpenedSubscription: Subscription;
  private proposedCategoryModalOpenedSubscription: Subscription;
  private addIngredientSubscription: Subscription;
  private addProposedIngredientSubscription: Subscription;
  private addProposedCategorySubscription: Subscription;
  private cancelProposedCategorySubscription: Subscription;

  constructor(
    private configStore: ConfigStore,
    private categoryService: CategoryService,
    private ingredientService: IngredientService,
    private ingredientCategoryService: IngredientCategoryService,
    private unitService: UnitService,
    private windowService: WindowService,
    private alertService: AlertService,
    private recipeService: RecipeService
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
    this.selectedCategory = this.categories[0];
    this.proposedRecipe.difficulty = this.difficulties[0];
    this.uploadFileRestrictions = this.configStore.getImageUploadFileRestriction();
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
      this.addIngredientsModalOpened = isOpened;
    });

    this.proposedIngredientModalOpenedSubscription = this.windowService.proposeIngredientWindowOpen.subscribe(isOpened => {
      this.addProposedIngredientModalOpened = isOpened;
    })

    this.proposedCategoryModalOpenedSubscription = this.windowService.proposeCategoryWindowOpen.subscribe(isOpened => {
      this.addProposedCategoryModalOpened = isOpened;
    })

    this.addIngredientSubscription = this.ingredientService.addIngredientModal.subscribe((ingredientPosition: IngredientPosition) =>{
      let ingredientQuantity = new IngredientQuantity();
      ingredientQuantity.ingredient = ingredientPosition.ingredient;

      if(ingredientPosition.isSelected){
        ingredientQuantity.unit = this.units[0]
        this.ingredientQuantities.push(ingredientQuantity);
      } else{
        this.ingredientQuantities.splice(this.ingredientQuantities.indexOf(ingredientQuantity), 1);
      }
    });

    this.addProposedIngredientSubscription = this.ingredientService.proposeIngredientModal.subscribe((ingredientPosition: IngredientPosition) => {
      let proposedIngredientQuantity = new ProposedIngredientQuantity(ingredientPosition.ingredient);
      if(ingredientPosition.isNew){
        proposedIngredientQuantity.unit = this.units[0];
        this.proposedIngredientQuantities.push(proposedIngredientQuantity);
      }
    });

    this.addProposedCategorySubscription = this.categoryService.proposeCategoryModal.subscribe((proposedCategory: ProposedCategory) => {
      this.proposedCategory = proposedCategory;
    });

    this.cancelProposedCategorySubscription = this.categoryService.proposeCategoryModalCancel.subscribe(() => {
      this.removeProposedCategory();
    })
  }

  disposeSubscriptions(){
    this.modalOpenedSubscription.unsubscribe();
    this.addIngredientSubscription.unsubscribe();
    this.addProposedIngredientSubscription.unsubscribe();
    this.proposedCategoryModalOpenedSubscription.unsubscribe();
    this.proposedIngredientModalOpenedSubscription.unsubscribe();
    this.addProposedCategorySubscription.unsubscribe();
    this.cancelProposedCategorySubscription.unsubscribe();
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

  openProposedIngredientModal(){
    this.ingredientPosition = new IngredientPosition(new Ingredient());
    this.windowService.proposeIngredientWindowOpen.next(true);
  }

  openProposedCategoryModal(){
    this.proposedCategory = this.proposedCategory ? this.proposedCategory : new ProposedCategory();
    this.windowService.proposeCategoryWindowOpen.next(true);
  }

  editProposedIngredient(proposedIngredient){
    let ingredientPosition = new IngredientPosition(proposedIngredient, null, true, false);
    console.log(ingredientPosition);
    this.ingredientPosition = ingredientPosition;
    this.windowService.proposeIngredientWindowOpen.next(true);
  }

  removeProposedIngredient(index: number){
    this.proposedIngredientQuantities.splice(index, 1);
  }

  removeIngredient(index: number){
    this.ingredientPositions.filter(x => x.ingredient.id == this.ingredientQuantities[index].ingredient.id)[0].isSelected = false;
    this.ingredientQuantities.splice(index, 1);
  }

  removeProposedCategory(){
    this.proposedCategory = null;
  }

  setStepsOrder(){
    for(let i = 0; i < this.steps.length; i++){
      this.steps[i].order = i;
    }
  }

  async submit(){
    if(this.validate()){
      this.configStore.startLoadingPanel();

      this.proposedRecipe.ingredientQuantities = this.ingredientQuantities;
      this.proposedRecipe.proposedIngredientQuantities = this.proposedIngredientQuantities;
      this.proposedRecipe.category = this.selectedCategory;
      this.proposedRecipe.proposedCategory = this.proposedCategory;
      this.setStepsOrder();
      this.proposedRecipe.steps = this.steps;

      await this.recipeService.addProposedRecipe(this.proposedRecipe).toPromise();
      console.log(this.proposedRecipe);
      console.log(JSON.stringify(this.proposedRecipe));

      this.configStore.stopLoadingPanel();
    }
  }

  validate(){
    if(!this.selectedCategory){
      this.alertService.showError("Nie wybrano kategorii")
      return false;
    } else if(!this.proposedRecipe.name || this.proposedRecipe.name.length < 3){
      this.alertService.showError("Nazwa musi zawierać conajmniej 3 znaki");
      return false;
    } else if(!this.proposedRecipe.time || this.proposedRecipe.time < 1){
      this.alertService.showError("Nie podano czasu przygotowania");
      return false;
    } else if(!this.proposedRecipe.personQuantity || this.proposedRecipe.personQuantity < 1){
      this.alertService.showError("Nie podano ilości osób");
      return false;
    } else if(!this.proposedRecipe.difficulty){
      this.alertService.showError("Nie podano poziomu trudności");
      return false;
    } else if(this.ingredientQuantities.length < 1 && this.proposedIngredientQuantities.length < 1){
      this.alertService.showError("Nie podano składników");
      return false;
    } else if(this.steps.length < 1){
      this.alertService.showError("Nie podano kroków");
      return false;
    } else if(!this.proposedRecipe.image){
      this.alertService.showError("Nie załadowano obrazu");
      return false;
    }

    for(let proposedIngredientQuantity of this.proposedIngredientQuantities){
      if(proposedIngredientQuantity.quantity <= 0){
        this.alertService.showError("W jednym ze składników podano zerową lub ujemną ilość");
        return false;
      }
    }

    for(let ingredientQuantity of this.ingredientQuantities){
      if(ingredientQuantity.quantity <= 0){
        this.alertService.showError("W jednym ze składników podano zerową lub ujemną ilość");
        return false;
      }
    }

    for(let step of this.steps){
      if(!step.description || step.description.length < 10){
        this.alertService.showError("Opis kroku powinien zawierać conajmniej 10 znaków");
        return false;
      }
    }

    return true;
  }

  async onSelectStepImageFile(event: SelectEvent, index: number){
    if(event.files[0].size < this.uploadFileRestrictions.maxFileSize){
      this.steps[index].image = await FileService.ConvertToBase64(event.files[0].rawFile) as string;
    } else{
      this.steps[index].image = null;
    }
  }

  async onSelectRecipeImageFile(event: SelectEvent){
    if(event.files[0].size < this.uploadFileRestrictions.maxFileSize){
      this.proposedRecipe.image = await FileService.ConvertToBase64(event.files[0].rawFile) as string;
    } else{
      this.proposedRecipe.image = null;
    }
  }
}
