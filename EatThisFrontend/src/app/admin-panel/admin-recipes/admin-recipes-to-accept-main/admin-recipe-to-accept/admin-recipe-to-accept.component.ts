import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { DialogCloseResult, DialogRef, DialogResult, DialogService } from '@progress/kendo-angular-dialog';
import { ConfigStore } from 'src/app/app-config/config-store';
import { DiscardRecipeRequest } from 'src/app/models/app-models/discard-recipe-request.model';
import { IngredientQuantity } from 'src/app/models/ingredient-quantity.model';
import { Ingredient } from 'src/app/models/ingredient.model';
import { ProposedCategory } from 'src/app/models/proposed-recipe/proposed-category.model';
import { ProposedIngredientQuantity } from 'src/app/models/proposed-recipe/proposed-ingredient-quantity.model';
import { ProposedIngredient } from 'src/app/models/proposed-recipe/proposed-ingredient.model';
import { ProposedRecipe } from 'src/app/models/proposed-recipe/proposed-recipe.model';
import { AlertService } from 'src/app/services/app-services/alert.service';
import { IngredientService } from 'src/app/services/ingredient.service';
import { RecipeService } from 'src/app/services/recipe.service';
import { ChangeIngredientModalComponent } from './change-ingredient-modal/change-ingredient-modal.component';
import { DiscardProposedRecipeReasonModalComponent } from './discard-proposed-recipe-reason-modal/discard-proposed-recipe-reason-modal.component';

@Component({
  selector: 'app-admin-recipe-to-accept',
  templateUrl: './admin-recipe-to-accept.component.html',
  styleUrls: ['./admin-recipe-to-accept.component.scss']
})
export class AdminRecipeToAcceptComponent implements OnInit {
  public proposedRecipe: ProposedRecipe;
  public ingredients: Ingredient[];

  private id: number;

  constructor(
    private route: ActivatedRoute,
    private configStore: ConfigStore,
    private recipeService: RecipeService,
    private ingredientService: IngredientService,
    private dialogService: DialogService,
    private alertService: AlertService,
    private router: Router
  ) { }

  async ngOnInit(): Promise<void> {
    this.route.paramMap.subscribe(() => {
      this.id = +this.route.snapshot.paramMap.get('id');
    });

    this.configStore.startLoadingPanel();
    await Promise.all([this.getProposedRecipe(), this.getIngredients()]);
    this.configStore.stopLoadingPanel();

    console.log(this.proposedRecipe);
  }

  async getProposedRecipe(){
    this.proposedRecipe = await this.recipeService.getProposedRecipeById(this.id).toPromise();
    this.proposedRecipe.proposedSteps = this.proposedRecipe.proposedSteps.sort((a, b) => {
      const orderDiff = a.order - b.order;
      if(orderDiff) return orderDiff;
      return null;
    });
  }

  async getIngredients(){
    this.ingredients = await this.ingredientService.getAll().toPromise();
  }

  async updateIngredientQuantitiesList(){
    const vm = await this.ingredientService.getIngredientQuantietiesByProposedRecipeId(this.proposedRecipe.id).toPromise();
    this.proposedRecipe.ingredientQuantities = vm.ingredientQuantities;
    this.proposedRecipe.proposedIngredientQuantities = vm.proposedIngredientQuantities;
  }

  acceptIngredient(proposedIngredient: ProposedIngredient){
    const dialog: DialogRef = this.dialogService.open({
      title: "Zaakceptuj składnik",
      content: `Czy jesteś pewny, że chcesz dodać proponowany składnik "${proposedIngredient.name}"?`,
      actions: [{ text: "Tak", accepted: true, themeColor: "primary" }, { text: "Nie" }],
      width: 450,
      minWidth: 250,
    })

    dialog.result.subscribe(async (result: any) => {
      if(result.accepted){
        this.configStore.startLoadingPanel();
        await this.ingredientService.acceptProposedIngredient(proposedIngredient.id).toPromise();
        await this.updateIngredientQuantitiesList();
        this.configStore.stopLoadingPanel();
      }
    })
  }

  removeIngredient(proposedIngredient: ProposedIngredient){
    const dialog: DialogRef = this.dialogService.open({
      title: "Usuń składnik",
      content: `Czy jesteś pewny, że chcesz usunąć proponowany składnik "${proposedIngredient.name}"? Spowoduje to również usunięcie pozycji na głównej liście składników`,
      actions: [{ text: "Tak", accepted: true, themeColor: "primary" }, { text: "Nie" }],
      width: 450,
      minWidth: 250,
    });

    dialog.result.subscribe(async (result: any) => {
      if(result.accepted){
        this.configStore.startLoadingPanel();
        await this.ingredientService.deleteProposedIngredient(proposedIngredient.id).toPromise();
        await this.updateIngredientQuantitiesList();
        this.configStore.stopLoadingPanel();
      }
    })
  }

  acceptCategory(proposedCategory: ProposedCategory){
    const dialog: DialogRef = this.dialogService.open({
      title: "Zaakceptuj kategorię",
      content: `Czy jesteś pewny, że chcesz dodać proponowany kategorię "${proposedCategory.name}"?`,
      actions: [{ text: "Tak", accepted: true, themeColor: "primary" }, { text: "Nie" }],
      width: 450,
      minWidth: 250,
    })

    dialog.result.subscribe(async (result: any) => {
      if(result.accepted){
        this.configStore.startLoadingPanel();
        await this.recipeService.acceptProposedCategory(this.proposedRecipe.id, this.proposedRecipe.proposedCategory).toPromise();
        await this.getProposedRecipe();
        this.configStore.stopLoadingPanel();
      }
    })
  }

  removeProposedCategory(proposedCategory: ProposedCategory){
    const dialog: DialogRef = this.dialogService.open({
      title: "Usuń kategorię",
      content: `Czy jesteś pewny, że chcesz usunąć proponowaną kategorię "${proposedCategory.name}"?`,
      actions: [{ text: "Tak", accepted: true, themeColor: "primary" }, { text: "Nie" }],
      width: 450,
      minWidth: 250,
    });

    dialog.result.subscribe(async (result: any) => {
      if(result.accepted){
        this.configStore.startLoadingPanel();
        await this.recipeService.removeProposedCategoryFromProposedRecipe(this.proposedRecipe.id, proposedCategory.id).toPromise();
        await this.getProposedRecipe();
        this.configStore.stopLoadingPanel();
      }
    })
  }

  changeIngredient(proposedIngredientQuantity: ProposedIngredientQuantity){
    const dialog: DialogRef = this.dialogService.open({
      title: "Zmień składnik",
      content: ChangeIngredientModalComponent,
      actions: [{ text: "Zmień", accepted: true, themeColor: "primary" }, { text: "Anuluj" }],
      width: 450,
      minWidth: 250,
    });

    const ingredientInfo = dialog.content.instance;
    ingredientInfo.proposedIngredient = proposedIngredientQuantity.proposedIngredient;
    ingredientInfo.ingredients = this.ingredients;
    ingredientInfo.changedIngredient = this.ingredients[0];

    dialog.result.subscribe(async (result: any) => {
      if(result.accepted){
        this.configStore.startLoadingPanel();
        await this.recipeService.changeProposedIngredientToIngredient(
          this.proposedRecipe.id, 
          proposedIngredientQuantity.proposedIngredient.id, 
          ingredientInfo.changedIngredient.id
          ).toPromise();
        await this.updateIngredientQuantitiesList();
        this.configStore.stopLoadingPanel();
      }
    })
  }

  async acceptRecipe(){
    if(this.proposedRecipe.proposedIngredientQuantities.length > 0 || this.proposedRecipe.proposedCategory){
      this.alertService.showError("Należy zaakceptować lub odrzucić zaproponowane obiekty");
      return;
    } else{
      this.configStore.startLoadingPanel();
      await this.recipeService.acceptProposedRecipe(this.proposedRecipe).toPromise();
      this.configStore.stopLoadingPanel();
      this.alertService.showSuccess("Zaakceptowano przepis");

      this.router.navigate(["../"], {relativeTo: this.route});
    }
  }

  discardRecipe(){
    const dialog: DialogRef = this.dialogService.open({
      title: "Podaj powód odrzucenia",
      content: DiscardProposedRecipeReasonModalComponent,
      actions: [{ text: "Odrzuć", accepted: true, themeColor: "primary" }, { text: "Anuluj" }],
      width: 450,
      minWidth: 250,
      preventAction: (ev) => {
        if(dialog.content.instance.message.length < 10){
          this.alertService.showError("Wiadomość powinna zawierać conajmniej 10 znaków");
          return true;
        }
        return false;
      }
    });

    const messageInfo = dialog.content.instance;
    messageInfo.message = "";

    dialog.result.subscribe(async (result: any) => {
      if(result.accepted){
        let request: DiscardRecipeRequest = new DiscardRecipeRequest();
        request.proposedRecipeId = this.proposedRecipe.id;
        request.message = messageInfo.message;
        request.email = this.proposedRecipe.userDetails.email;
        
        this.configStore.startLoadingPanel();
        await this.recipeService.discardProposedRecipe(request).toPromise();
        this.configStore.stopLoadingPanel();

        this.alertService.showInfo("Odrzucono przepis");
        this.router.navigate(["../"], {relativeTo: this.route});
      }
    });
  }
}
