import { Component, OnInit, ViewChild } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { PageChangeEvent } from '@progress/kendo-angular-dropdowns/dist/es2015/common/models/page-change-event';
import { DataBindingDirective, GridDataResult } from '@progress/kendo-angular-grid';
import { Subscription } from 'rxjs';
import { Category } from 'src/app/models/category.model';
import { CategoryService } from 'src/app/services/category.service';
import { DeleteObject, WindowService } from 'src/app/services/window.service';
import { process } from "@progress/kendo-data-query";

@Component({
  selector: 'app-categories',
  templateUrl: './categories.component.html',
  styleUrls: ['./categories.component.scss']
})
export class CategoriesComponent implements OnInit {
  @ViewChild(DataBindingDirective) dataBinding: DataBindingDirective;
  public gridView: GridDataResult;
  public categories: Category[];
  public gridViewSearch: any[];
  public isCreateModalOpened = false;
  public deleteModalOpened = false;
  public pageSize = 15;
  public skip = 0;
  public isNew = false;
  public category: Category;

  public formGroup: FormGroup;
  public loadingPanelVisible = false;

  private windowCloseListener: Subscription;
  private categorySaveListener: Subscription;
  private categoryDeleteListener: Subscription;

  constructor(private categoryService: CategoryService, private windowService: WindowService) {
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
    this.categorySaveListener.unsubscribe();
    this.windowService.deleteObject.unsubscribe();
  }

  private async addListeners(){
    this.windowCloseListener = this.windowService.closeWindow.subscribe((value) => {
      this.isCreateModalOpened = value;
      this.deleteModalOpened = value;
    })
    this.categorySaveListener = this.categoryService.createCategoryModalSaved.subscribe(async category => {
      if(this.isNew){
        await this.saveCategory(category);
        console.log("Log");
      }else{
        await this.updateCategory(category);
      }
    });
    this.categoryDeleteListener = this.windowService.deleteObject.subscribe(async object => {
      await this.deleteCategory(object);
    })
  }

  private async deleteCategory(deleteObject: DeleteObject){
    this.loadingPanelVisible = true;
    this.category.id = deleteObject.id;
    this.category.name = deleteObject.name;
    await this.categoryService.delete(this.category).toPromise();
    this.loadingPanelVisible = false;
    await this.ngOnInit();
  }

  private async saveCategory(category: Category){
    this.loadingPanelVisible = true;
    await this.categoryService.add(category).toPromise();
    this.isCreateModalOpened = false;
    this.loadingPanelVisible = false;
    await this.ngOnInit();
  }

  private async updateCategory(category: Category){
    this.loadingPanelVisible = true;
    this.isNew = false;
    await this.categoryService.update(category).toPromise();
    this.isCreateModalOpened = false;
    this.loadingPanelVisible = false;
    await this.ngOnInit();
  }

  private async getAll(){
    this.loadingPanelVisible = true;
    this.categories = await this.categoryService.getAll().toPromise();
    this.loadingPanelVisible = false;
  }

  public addHandler() {
    this.isCreateModalOpened = true;
    this.isNew = true;
    this.category = {id: 0, name: ""}
  }

  public editHandler({dataItem }: any) {
    this.isCreateModalOpened = true;
    this.category = dataItem;
    this.isNew = false;
  }

  public removeHandler({ dataItem }: any) {
    this.deleteModalOpened = true;
    this.category = dataItem;
  }


  public async pageChange(event: PageChangeEvent){
    this.skip = event.skip;
    await this.loadData();
  }

  private async loadData(): Promise<void>{
    this.gridView = {
      data: this.categories.slice(this.skip, this.skip + this.pageSize),
      total: this.categories.length,
    }
    let event: any = { target: { value: "" } };
    this.onFilter(event);
  }

  public onFilter(event: any): void {
    this.gridViewSearch = process(this.categories, {
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
