import { Component, Input, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { Category } from 'src/app/models/category.model';
import { CategoryService } from 'src/app/services/category.service';
import { WindowService } from 'src/app/services/window.service';

@Component({
  selector: 'app-categories-form-modal',
  templateUrl: './categories-form-modal.component.html',
  styleUrls: ['./categories-form-modal.component.scss']
})
export class CategoriesFormModalComponent implements OnInit {
  @Input() public opened: boolean;
  @Input() public isNew: boolean;
  @Input() public category: Category;
  public form: FormGroup = new FormGroup({
    id: new FormControl(),
    name: new FormControl()
  });

  constructor(private categoryService: CategoryService, private windowService: WindowService) { }

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
    this.category = this.form.value;
    await this.categoryService.createCategoryModalSaved.next(this.category);
    this.close();
  }
}
