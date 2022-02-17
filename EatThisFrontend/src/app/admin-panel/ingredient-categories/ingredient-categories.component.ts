import { Component, OnInit, ViewChild } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { DataBindingDirective, GridDataResult, PageChangeEvent } from '@progress/kendo-angular-grid';
import { Subscription } from 'rxjs';
import { IngredientCategory } from 'src/app/models/ingredient-category.model';
import { IngredientCategoryService } from 'src/app/services/ingredient-category.service';
import { DeleteObject, WindowService } from 'src/app/services/window.service';
import { process } from "@progress/kendo-data-query";

@Component({
  selector: 'app-ingredient-categories',
  templateUrl: './ingredient-categories.component.html',
  styleUrls: ['./ingredient-categories.component.scss']
})
export class IngredientCategoriesComponent implements OnInit {
  @ViewChild(DataBindingDirective) dataBinding: DataBindingDirective;
  public gridView: GridDataResult;
  public ingredientCategories: IngredientCategory[];
  public gridViewSearch: any[];
  public isCreateModalOpened = false;
  public deleteModalOpened = false;
  public pageSize = 15;
  public skip = 0;
  public isNew = false;
  public ingredientCategory: IngredientCategory;

  public formGroup: FormGroup;
  public loadingPanelVisible = false;

  private windowCloseListener: Subscription;
  private ingredientCategorySaveListener: Subscription;
  private ingredientCategoryDeleteListener: Subscription;

  constructor(private ingredientCategoryService: IngredientCategoryService, private windowService: WindowService) {
    this.addListeners();
  }

  public async ngOnInit(): Promise<void> {
    await this.getAll();
    await this.loadData();
  }

  async ngOnDestroy(): Promise<void> {
    await this.removeListeners()
  }

  private async removeListeners(){
    this.windowCloseListener.unsubscribe();
    this.ingredientCategorySaveListener.unsubscribe();
    this.ingredientCategoryDeleteListener.unsubscribe();
  }

  private async addListeners(){
    this.windowCloseListener = this.windowService.closeWindow.subscribe((value) => {
      this.isCreateModalOpened = value;
      this.deleteModalOpened = value;
    })
    this.ingredientCategorySaveListener = this.ingredientCategoryService.createIngredientCategoryModalSaved.subscribe(async ingredientCategory => {
      if(this.isNew){
        await this.saveIngredientCategory(ingredientCategory);
        console.log("Log");
      }else{
        await this.updateIngredientCategory(ingredientCategory);
      }
    });
    this.ingredientCategoryDeleteListener = this.windowService.deleteObject.subscribe(async object => {
      await this.deleteIngredientCategory(object);
    })
  }

  private async deleteIngredientCategory(deleteObject: DeleteObject){
    this.loadingPanelVisible = true;
    this.ingredientCategory.id = deleteObject.id;
    this.ingredientCategory.name = deleteObject.name;
    await this.ingredientCategoryService.delete(this.ingredientCategory)
    this.loadingPanelVisible = false;
    await this.ngOnInit();
  }

  private async saveIngredientCategory(ingredientCategory: IngredientCategory){
    this.loadingPanelVisible = true;
    await this.ingredientCategoryService.add(ingredientCategory)
    this.isCreateModalOpened = false;
    this.loadingPanelVisible = false;
    await this.ngOnInit();
  }

  private async updateIngredientCategory(ingredientCategory: IngredientCategory){
    this.loadingPanelVisible = true;
    this.isNew = false;
    await this.ingredientCategoryService.update(ingredientCategory)
    this.isCreateModalOpened = false;
    this.loadingPanelVisible = false;
    await this.ngOnInit();
  }

  private async getAll(){
    this.loadingPanelVisible = true;
    this.ingredientCategories = await this.ingredientCategoryService.getAll().toPromise();
    this.loadingPanelVisible = false;
  }

  public addHandler() {
    this.isCreateModalOpened = true;
    this.isNew = true;
    this.ingredientCategory = {id: 0, name: ""}
  }

  public editHandler({dataItem }: any) {
    this.isCreateModalOpened = true;
    this.ingredientCategory = dataItem;
    this.isNew = false;
  }

  public removeHandler({ dataItem }: any) {
    this.deleteModalOpened = true;
    this.ingredientCategory = dataItem;
  }


  public async pageChange(event: PageChangeEvent){
    this.skip = event.skip;
    await this.loadData();
  }

  private async loadData(): Promise<void>{
    this.gridView = {
      data: this.ingredientCategories.slice(this.skip, this.skip + this.pageSize),
      total: this.ingredientCategories.length,
    }
    let event: any = { target: { value: "" } };
    this.onFilter(event);
  }

  public onFilter(event: any): void {
    this.gridViewSearch = process(this.ingredientCategories, {
      filter: {
        logic: "or",
        filters: [
          {
            field: "name",
            operator: "contains",
            value: event.target.value,
          }
        ],
      },
    }).data;

    this.dataBinding.skip = 0;
  }

}
