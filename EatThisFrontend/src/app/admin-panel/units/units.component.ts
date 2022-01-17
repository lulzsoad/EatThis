import { Component, OnInit, ViewChild } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { DataBindingDirective, GridDataResult, PageChangeEvent } from '@progress/kendo-angular-grid';
import { process } from "@progress/kendo-data-query";
import { Subscription } from 'rxjs';
import { Unit } from 'src/app/models/unit.model';
import { UnitService } from 'src/app/services/unit.service';
import { DeleteObject, WindowService } from 'src/app/services/window.service';

@Component({
  selector: 'app-units',
  templateUrl: './units.component.html',
  styleUrls: ['./units.component.scss']
})
export class UnitsComponent implements OnInit {
  @ViewChild(DataBindingDirective) dataBinding: DataBindingDirective;
  public gridView: GridDataResult;
  public units: Unit[];
  public gridViewSearch: any[];
  public isCreateModalOpened = false;
  public deleteModalOpened = false;
  public pageSize = 15;
  public skip = 0;
  public isNew = false;
  public unit: Unit;

  public formGroup: FormGroup;
  public loadingPanelVisible = false;

  private windowCloseListener: Subscription;
  private unitSaveListener: Subscription;
  private unitDeleteListener: Subscription;

  constructor(private unitService: UnitService, private windowService: WindowService) {
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
    this.unitSaveListener.unsubscribe();
    this.unitDeleteListener.unsubscribe();
  }

  private async addListeners(){
    this.windowCloseListener = this.windowService.closeWindow.subscribe((value) => {
      this.isCreateModalOpened = value;
      this.deleteModalOpened = value;
    })
    this.unitSaveListener = this.unitService.createUnitModalSaved.subscribe(async unit => {
      if(this.isNew){
        await this.saveUnit(unit);
      }else{
        await this.updateUnit(unit);
      }
    });
    this.unitDeleteListener = this.windowService.deleteObject.subscribe(async object => {
      await this.deleteUnit(object);
    })
  }

  private async deleteUnit(deleteObject: DeleteObject){
    this.loadingPanelVisible = true;
    this.unit.id = deleteObject.id;
    this.unit.name = deleteObject.name;
    await this.unitService.delete(this.unit)
    await this.ngOnInit();
    this.loadingPanelVisible = false;
  }

  private async saveUnit(unit: Unit){
    this.loadingPanelVisible = true;
    await this.unitService.add(unit);
    await this.ngOnInit();
    this.isCreateModalOpened = false;
    this.loadingPanelVisible = false;
  }

  private async updateUnit(unit: Unit){
    this.loadingPanelVisible = true;
    this.isNew = false;
    await this.unitService.update(unit)
    await this.ngOnInit();
    this.isCreateModalOpened = false;
    this.loadingPanelVisible = false;
  }

  private async getAll(){
    this.loadingPanelVisible = true;
    this.units = await this.unitService.getAll().toPromise();
    this.loadingPanelVisible = false;
  }

  public addHandler() {
    this.isCreateModalOpened = true;
    this.isNew = true;
    this.unit = {id: 0, name: ""}
  }

  public editHandler({dataItem }: any) {
    this.isCreateModalOpened = true;
    this.unit = dataItem;
    this.isNew = false;
  }

  public removeHandler({ dataItem }: any) {
    this.deleteModalOpened = true;
    this.unit = dataItem;
  }


  public async pageChange(event: PageChangeEvent){
    this.skip = event.skip;
    await this.loadData();
  }

  private async loadData(): Promise<void>{
    this.gridView = {
      data: this.units.slice(this.skip, this.skip + this.pageSize),
      total: this.units.length,
    }
    let event: any = { target: { value: "" } };
    this.onFilter(event);
  }

  public onFilter(event: any): void {
    this.gridViewSearch = process(this.units, {
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
