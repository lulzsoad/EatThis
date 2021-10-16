import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { DataBindingDirective, GridDataResult, PageChangeEvent } from '@progress/kendo-angular-grid';
import { State } from '@progress/kendo-data-query';
import { Observable, Subscription } from 'rxjs';
import { Ingredient } from 'src/app/models/ingredient.model';
import { IngredientService } from 'src/app/services/ingredient.service';
import { process } from "@progress/kendo-data-query";
import { DeleteObject, WindowService } from 'src/app/services/window.service';

@Component({
  selector: 'app-ingredients',
  templateUrl: './ingredients.component.html',
  styleUrls: ['./ingredients.component.scss']
})
export class IngredientsComponent implements OnInit, OnDestroy {
  @ViewChild(DataBindingDirective) dataBinding: DataBindingDirective;
  public gridView: GridDataResult;
  public ingredients: Ingredient[];
  public gridViewSearch: any[];
  public isCreateModalOpened = false;
  public deleteModalOpened = false;
  public pageSize = 15;
  public skip = 0;
  public isNew = false;
  public ingredient: Ingredient;

  public formGroup: FormGroup;
  public loadingPanelVisible = false;

  private windowCloseListener: Subscription;
  private ingredientSaveListener: Subscription;
  private ingredientDeleteListener: Subscription;

  constructor(private ingredientService: IngredientService, private windowService: WindowService) {
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
    this.ingredientSaveListener.unsubscribe();
    this.windowService.deleteObject.unsubscribe();
  }

  private async addListeners(){
    this.windowCloseListener = this.windowService.closeWindow.subscribe((value) => {
      this.isCreateModalOpened = value;
      this.deleteModalOpened = value;
    })
    this.ingredientSaveListener = this.ingredientService.createIngredientModalSaved.subscribe(async ingredient => {
      if(this.isNew){
        await this.saveIngredient(ingredient);
      }else{
        await this.updateIngredient(ingredient);
      }
    });
    this.ingredientDeleteListener = this.windowService.deleteObject.subscribe(async object => {
      await this.deleteIngredient(object);
    })
  }

  private async deleteIngredient(deleteObject: DeleteObject){
    this.loadingPanelVisible = true;
    this.ingredient.id = deleteObject.id;
    this.ingredient.name = deleteObject.name;
    await this.ingredientService.delete(this.ingredient).toPromise();
    await this.ngOnInit();
    this.loadingPanelVisible = false;
  }

  private async saveIngredient(ingredient: Ingredient){
    this.loadingPanelVisible = true;
    await this.ingredientService.add(ingredient).toPromise();
    await this.ngOnInit();
    this.isCreateModalOpened = false;
    this.loadingPanelVisible = false;
  }

  private async updateIngredient(ingredient: Ingredient){
    this.loadingPanelVisible = true;
    this.isNew = false;
    await this.ingredientService.update(ingredient).toPromise();
    await this.ngOnInit();
    this.isCreateModalOpened = false;
    this.loadingPanelVisible = false;
  }

  private async getAll(){
    this.loadingPanelVisible = true;
    this.ingredients = await this.ingredientService.getAll().toPromise();
    this.loadingPanelVisible = false;
  }

  public addHandler() {
    this.isCreateModalOpened = true;
    this.isNew = true;
    this.ingredient = {id: 0, name: ""}
  }

  public editHandler({dataItem }: any) {
    this.isCreateModalOpened = true;
    this.ingredient = dataItem;
    this.isNew = false;
  }

  public removeHandler({ dataItem }: any) {
    this.deleteModalOpened = true;
    this.ingredient = dataItem;
  }


  public async pageChange(event: PageChangeEvent){
    this.skip = event.skip;
    await this.loadData();
  }

  private async loadData(): Promise<void>{
    this.gridView = {
      data: this.ingredients.slice(this.skip, this.skip + this.pageSize),
      total: this.ingredients.length,
    }
    let event: any = { target: { value: "" } };
    this.onFilter(event);
  }

  public onFilter(event: any): void {
    this.gridViewSearch = process(this.ingredients, {
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
