import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ConfigStore } from 'src/app/app-config/config-store';
import { ProposedRecipe } from 'src/app/models/proposed-recipe/proposed-recipe.model';
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
    private recipeService: RecipeService,
    private configStore: ConfigStore
  ) { }

  async ngOnInit(): Promise<void> {
    this.route.paramMap.subscribe(() => {
      this.id = +this.route.snapshot.paramMap.get('id');
    });

    console.log(this.id);
    this.configStore.startLoadingPanel();
    this.proposedRecipe = await this.recipeService.getProposedRecipeById(this.id).toPromise();
    this.configStore.stopLoadingPanel();

    console.log(this.proposedRecipe);
  }

}
