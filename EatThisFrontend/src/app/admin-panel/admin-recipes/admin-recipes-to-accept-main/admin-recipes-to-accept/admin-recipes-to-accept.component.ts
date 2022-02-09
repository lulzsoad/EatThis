import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { PagerSettings } from '@progress/kendo-angular-grid';
import { ConfigStore } from 'src/app/app-config/config-store';
import { DataChunk } from 'src/app/models/app-models/data-chunk.model';
import { ProposedRecipe } from 'src/app/models/proposed-recipe/proposed-recipe.model';
import { RecipeService } from 'src/app/services/recipe.service';

@Component({
  selector: 'app-admin-recipes-to-accept',
  templateUrl: './admin-recipes-to-accept.component.html',
  styleUrls: ['./admin-recipes-to-accept.component.scss']
})
export class AdminRecipesToAcceptComponent implements OnInit {
  public proposedRecipes: DataChunk<ProposedRecipe>;
  public skip: number = 0;
  public take: number = 20;
  public pageableConfig: PagerSettings = {
    buttonCount: 5,
    type: "numeric",
    info: true,
    previousNext: true,
    position: "bottom"
  }

  constructor(
    private recipeService: RecipeService,
    private router: Router,
    private route: ActivatedRoute
    ) { }

  async ngOnInit(): Promise<void> {
    await this.getProposedRecipes();
  }

  private async getProposedRecipes(){
    this.proposedRecipes = await this.recipeService.getChunkOfProposedRecipes(this.skip, this.take).toPromise();
    console.log(this.proposedRecipes);
  }

  async pageChange(event){
    this.take = event.take;
    this.skip = event.skip;
    this.getProposedRecipes();
    console.log(this.proposedRecipes);
  }

  navigateToRecipe(id){
    this.router.navigate([`${id}`], {relativeTo: this.route});
  }
}
