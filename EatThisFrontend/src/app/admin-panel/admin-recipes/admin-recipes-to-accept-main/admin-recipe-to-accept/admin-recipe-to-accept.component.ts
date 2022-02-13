import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { DialogCloseResult, DialogRef, DialogResult, DialogService } from '@progress/kendo-angular-dialog';
import { ConfigStore } from 'src/app/app-config/config-store';
import { IngredientQuantity } from 'src/app/models/ingredient-quantity.model';
import { ProposedIngredient } from 'src/app/models/proposed-recipe/proposed-ingredient.model';
import { ProposedRecipe } from 'src/app/models/proposed-recipe/proposed-recipe.model';
import { IngredientService } from 'src/app/services/ingredient.service';
import { RecipeService } from 'src/app/services/recipe.service';

@Component({
  selector: 'app-admin-recipe-to-accept',
  templateUrl: './admin-recipe-to-accept.component.html',
  styleUrls: ['./admin-recipe-to-accept.component.scss']
})
export class AdminRecipeToAcceptComponent implements OnInit {
  public proposedRecipe: ProposedRecipe;

  private id: number;

  constructor(
    private route: ActivatedRoute,
    private configStore: ConfigStore,
    private recipeService: RecipeService,
    private ingredientService: IngredientService,
    private dialogService: DialogService
  ) { }

  async ngOnInit(): Promise<void> {
    this.route.paramMap.subscribe(() => {
      this.id = +this.route.snapshot.paramMap.get('id');
    });

    this.configStore.startLoadingPanel();
    await this.getProposedRecipe();
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
}
