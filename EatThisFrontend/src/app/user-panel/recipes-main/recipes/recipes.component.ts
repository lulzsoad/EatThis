import { Route } from '@angular/compiler/src/core';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ConfigStore } from 'src/app/app-config/config-store';
import { DataChunk } from 'src/app/models/app-models/data-chunk.model';
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
export class RecipesComponent implements OnInit, OnDestroy{
  public bookmarks: Bookmark[];
  public categories: Category[];
  public recipes: Recipe[];
  public selectedBookmark: Bookmark;
  public page: number = 1;
  public count: number;
  public categoryId: number;
  public pages: Bookmark[] = [];
  public selectedPage: Bookmark;
  
  private take: number = 8;
  private skip: number = 0;
  

  constructor(
    private configStore: ConfigStore,
    private categoryService: CategoryService,
    private recipeService: RecipeService,
    private router: Router,
    private route: ActivatedRoute
  ) { }
  

  async ngOnInit(): Promise<void> {
    this.configStore.startLoadingPanel();
    await this.getCategories();
    this.configStore.stopLoadingPanel();
    this.fillBookmarks();
    this.initializeSubscriptions();
    
  }

  ngOnDestroy(): void {
    this.disposeSubscriptions();
  }

  initializeSubscriptions(){
    this.route.queryParams.subscribe(async params => {
      this.page = +params['page']
      this.categoryId = +params['categoryId'];

      if(this.bookmarks?.length > 0 && this.page){
        if(this.bookmarks.filter(x => x.isActive).length > 0){
          this.bookmarks.filter(x => x.isActive)[0].isActive = false;
        }
        this.selectedBookmark = (this.categoryId) ? this.bookmarks.filter(x => x.name == this.categoryId.toString())[0] : this.bookmarks.filter(x => x.name == '0')[0];
        this.bookmarks.filter(x => x.name == this.selectedBookmark.name)[0].isActive = true;

        this.configStore.startLoadingPanel();
        await this.getRecipes();
        this.loadPages();
        this.selectedPage = this.pages.filter(x => x.label == this.page.toString())[0];
       
        this.configStore.stopLoadingPanel();
      }
    })
  }

  async changePage(page: Bookmark){
    this.router.navigate([], {queryParams: {categoryId: this.categoryId, page: page.label}})
  }

  loadPages(){
    this.pages = [];
    let pagesCount = Math.ceil(this.count / this.take);
        for(let i = 0; i < pagesCount; i++){
          this.pages.push(new Bookmark(i.toString(), (i+1).toString()));
        }
  }

  disposeSubscriptions(){

  }

  async getRecipes(){
    const data: DataChunk<Recipe> = await this.recipeService.getChunkOfRecipesByCategory(this.categoryId.toString(), (this.page - 1) * this.take, this.take).toPromise();
    this.recipes = data.data;
    this.count = data.total;
  }

  async getCategories(){
    this.categories = await this.categoryService.getAll().toPromise();
  }

  fillBookmarks(){
    this.bookmarks = [];
    this.bookmarks.push(new Bookmark("0", "Wszystkie", false));
    for(let category of this.categories){
      this.bookmarks.push(new Bookmark(category.id.toString(), category.name));
    }
  }

  async changeBookmark(bookmark: Bookmark){
    if(bookmark.isDisabled)
      return;
    if(!this.page){
      this.page = 1;
    }
    
    this.selectedBookmark = bookmark;
    this.categoryId = +bookmark.name;
    this.page = 1;

    this.router.navigate([], {queryParams: {categoryId: this.categoryId, page: this.page}})
  }

}
