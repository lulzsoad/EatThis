import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ConfigStore } from 'src/app/app-config/config-store';
import { DataChunk } from 'src/app/models/app-models/data-chunk.model';
import { Recipe } from 'src/app/models/recipe.model';
import { RecipeService } from 'src/app/services/recipe.service';

@Component({
  selector: 'app-user-panel-main-page',
  templateUrl: './user-panel-main-page.component.html',
  styleUrls: ['./user-panel-main-page.component.scss']
})
export class UserPanelMainPageComponent implements OnInit {
  public recentRecipes: DataChunk<Recipe>;
  
  private skip: number = 0;
  private take: number = 6;

  constructor(
    private configStore: ConfigStore,
    private recipeService: RecipeService,
    private router: Router,
    private route: ActivatedRoute
  ) { }

  async ngOnInit(): Promise<void> {
    this.configStore.startLoadingPanel();
    await this.getRecentRecipes();
    this.configStore.stopLoadingPanel();
  }

  async getRecentRecipes(){
    this.recentRecipes = await this.recipeService.getChunkOfRecipesByCategory('', this.skip, this.take).toPromise();
  }

  navigateToRecipe(id: number){
    this.router.navigate(['recipes', id], {relativeTo: this.route})
  }
}
