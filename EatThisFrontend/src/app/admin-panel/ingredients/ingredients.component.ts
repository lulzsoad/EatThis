import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { GridDataResult } from '@progress/kendo-angular-grid';
import { State } from '@progress/kendo-data-query';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-ingredients',
  templateUrl: './ingredients.component.html',
  styleUrls: ['./ingredients.component.scss']
})
export class IngredientsComponent implements OnInit {

  public data = [];
  public gridState: State = {
    sort: [],
    skip: 0,
    take: 10,
  };
  public formGroup: FormGroup = new FormGroup({});
  private editedRowIndex: number = 0;

  constructor() {
  }

  public ngOnInit(): void {
  }

  public onStateChange(state: State) {
    this.gridState = state;
  }

  public addHandler({ sender }: any) {
    this.closeEditor(sender);

    this.formGroup = new FormGroup({
      ProductID: new FormControl(),
      ProductName: new FormControl("", Validators.required),
      UnitPrice: new FormControl(0),
      UnitsInStock: new FormControl(
        "",
        Validators.compose([
          Validators.required,
          Validators.pattern("^[0-9]{1,3}"),
        ])
      ),
      Discontinued: new FormControl(false),
    });

    sender.addRow(this.formGroup);
  }

  public editHandler({ sender, rowIndex, dataItem }: any) {
    this.closeEditor(sender);

    this.formGroup = new FormGroup({
      ProductID: new FormControl(dataItem.ProductID),
      ProductName: new FormControl(dataItem.ProductName, Validators.required),
      UnitPrice: new FormControl(dataItem.UnitPrice),
      UnitsInStock: new FormControl(
        dataItem.UnitsInStock,
        Validators.compose([
          Validators.required,
          Validators.pattern("^[0-9]{1,3}"),
        ])
      ),
      Discontinued: new FormControl(dataItem.Discontinued),
    });

    this.editedRowIndex = rowIndex;

    sender.editRow(rowIndex, this.formGroup);
  }

  public cancelHandler({ sender, rowIndex }: any) {
    this.closeEditor(sender, rowIndex);
  }

  public saveHandler({ sender, rowIndex, formGroup, isNew }: any) {

    sender.closeRow(rowIndex);
  }

  public removeHandler({ dataItem }: any) {
  }

  private closeEditor(grid: any, rowIndex = this.editedRowIndex) {
    grid.closeRow(rowIndex);
    this.editedRowIndex = 0;
    this.formGroup = new FormGroup({});
  }

}
