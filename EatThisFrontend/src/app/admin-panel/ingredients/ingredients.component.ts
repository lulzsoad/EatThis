import { Component, OnInit, ViewChild } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { DataBindingDirective, GridDataResult, PageChangeEvent } from '@progress/kendo-angular-grid';
import { State } from '@progress/kendo-data-query';
import { Observable } from 'rxjs';
import { Ingredient } from 'src/app/models/ingredient.model';
import { IngredientService } from 'src/app/services/ingredient.service';
import { process } from "@progress/kendo-data-query";

@Component({
  selector: 'app-ingredients',
  templateUrl: './ingredients.component.html',
  styleUrls: ['./ingredients.component.scss']
})
export class IngredientsComponent implements OnInit {
  @ViewChild(DataBindingDirective) dataBinding: DataBindingDirective;
  public gridView: GridDataResult;
  public ingredients: Ingredient[];
  public gridViewSearch: any[];
  public pageSize = 15;
  public skip = 0;

  public formGroup: FormGroup;
  public loadingPanelVisible = false;

  private editedRowIndex: number = 0;

  constructor(private ingredientService: IngredientService) {
  }

  public async ngOnInit(): Promise<void> {
    await this.getAll();
    await this.loadData()
  }

  private async getAll(){
    this.loadingPanelVisible = true;
    this.ingredients = await this.ingredientService.getAll().toPromise();
    this.loadingPanelVisible = false;
  }

  

  public addHandler({ sender }: any) {
    this.closeEditor(sender);

    this.formGroup = new FormGroup({
      id: new FormControl(),
      name: new FormControl(),
    });

    sender.addRow(this.formGroup);
  }

  public editHandler({ sender, rowIndex, dataItem }: any) {
    this.closeEditor(sender);
    this.formGroup = new FormGroup({
      id: new FormControl(dataItem.id),
      name: new FormControl(dataItem.name),
    });

    this.editedRowIndex = rowIndex;

    sender.editRow(rowIndex, this.formGroup);
  }

  public cancelHandler({ sender, rowIndex }: any) {
    this.closeEditor(sender, rowIndex);
  }

  public async saveHandler({ sender, rowIndex, formGroup, isNew }: any) {
    let ingredient: Ingredient = formGroup.value;
    if(isNew){
      this.loadingPanelVisible = true;
      ingredient.id = 0;
      await this.ingredientService.add(ingredient).toPromise().then(async (ingredientId) => {
        ingredient.id = ingredientId;
        this.ingredients.push(ingredient);
        await this.loadData();
        this.skip += 1;
      }) as number;
      this.loadingPanelVisible = false;
    } else{
      this.loadingPanelVisible = true;
      await this.ingredientService.update(ingredient).toPromise().then(async () => {
        await this.loadData();
        this.skip += 1;
        sender.data.data[rowIndex].name = formGroup.value.name;
      });
      this.loadingPanelVisible = false;
    }
    sender.closeRow(rowIndex);
  }

  public removeHandler({ dataItem }: any) {
  }

  private closeEditor(grid: any, rowIndex = this.editedRowIndex) {
    grid.closeRow(rowIndex);
    this.editedRowIndex = 0;
    this.formGroup = new FormGroup({});
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

  async dataStateChange(event: any){
    this.skip = event.skip;
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
