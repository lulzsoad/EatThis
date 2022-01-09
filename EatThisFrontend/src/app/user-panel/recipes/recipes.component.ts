import { Component, OnInit } from '@angular/core';
import { ConfigStore } from 'src/app/app-config/config-store';
import { Bookmark } from 'src/app/models/bookmark.model';
import { Category } from 'src/app/models/category.model';
import { Recipe } from 'src/app/models/recipe.model';
import { CategoryService } from 'src/app/services/category.service';
import { RecipeService } from 'src/app/services/recipe.service';

@Component({
  selector: 'app-recipes',
  templateUrl: './recipes.component.html',
  styleUrls: ['./recipes.component.scss']
})
export class RecipesComponent implements OnInit {
  public bookmarks: Bookmark[];
  public categories: Category[];
  public recipes: Recipe[];
  public selectedBookmark: Bookmark;
  public page: number;
  
  private take: number = 20;
  private skip: number = 0;

  constructor(
    private configStore: ConfigStore,
    private categoryService: CategoryService,
    private recipeService: RecipeService
  ) { }

  async ngOnInit(): Promise<void> {
    this.configStore.startLoadingPanel();
    await this.getCategories();
    this.fillBookmarks();
    this.configStore.stopLoadingPanel();
  }

  async getCategories(){
    this.categories = await this.categoryService.getAll().toPromise();
  }

  fillBookmarks(){
    this.bookmarks = [];
    this.bookmarks.push(new Bookmark("", "Wszystkie", false));
    for(let category of this.categories){
      this.bookmarks.push(new Bookmark(category.id.toString(), category.name));
    }
  }

  async changeBookmark(bookmark: Bookmark){
    if(bookmark.isDisabled)
      return;
    this.configStore.startLoadingPanel();
    if(this.bookmarks.filter(x => x.isActive).length > 0){
      this.bookmarks.filter(x => x.isActive)[0].isActive = false;
    }

    this.bookmarks.filter(x => x.name == bookmark.name)[0].isActive = true;
    this.selectedBookmark = bookmark;
    
    this.recipes = await this.recipeService.getRecipesByCategory(this.selectedBookmark.name, this.skip, this.take).toPromise();
    this.configStore.stopLoadingPanel();
  }

}
