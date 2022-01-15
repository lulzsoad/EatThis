import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ConfigStore } from 'src/app/app-config/config-store';
import { Recipe } from 'src/app/models/recipe.model';
import { RecipeService } from 'src/app/services/recipe.service';

@Component({
  selector: 'app-recipe',
  templateUrl: './recipe.component.html',
  styleUrls: ['./recipe.component.scss']
})
export class RecipeComponent implements OnInit {
  public recipe: Recipe;

  constructor(
    private route: ActivatedRoute,
    private recipeService: RecipeService,
    private configStore: ConfigStore
  ) 
  { }

  async ngOnInit(): Promise<void> {
    this.initializeSubscriptions();
  }

  initializeSubscriptions(){
    this.route.params.subscribe(async params => {
      let id: number = +params['id'];
      if(id){
        this.configStore.startLoadingPanel();
        this.recipe = await this.recipeService.getRecipeById(id).toPromise();
        console.log(this.recipe)
        this.configStore.stopLoadingPanel();
      }
    })
  }
}
