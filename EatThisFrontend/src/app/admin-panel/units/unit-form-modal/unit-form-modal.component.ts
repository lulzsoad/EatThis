import { Component, Input, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { Unit } from 'src/app/models/unit.model';
import { UnitService } from 'src/app/services/unit.service';
import { WindowService } from 'src/app/services/window.service';

@Component({
  selector: 'app-unit-form-modal',
  templateUrl: './unit-form-modal.component.html',
  styleUrls: ['./unit-form-modal.component.scss']
})
export class UnitFormModalComponent implements OnInit {
  @Input() public opened: boolean;
  @Input() public isNew: boolean;
  @Input() public unit: Unit;
  public form: FormGroup = new FormGroup({
    id: new FormControl(),
    name: new FormControl()
  });

  constructor(private unitService: UnitService, private windowService: WindowService) { }

  ngOnInit(): void {

  }

  public close() {
    this.opened = false;
    this.windowService.closeWindow.next(false);
  }

  public open() {
    this.opened = true;
  }

  public async submit() {
    this.unit = this.form.value;
    await this.unitService.createUnitModalSaved.next(this.unit);
    this.close();
  }
}
