import { Component, Input, OnInit } from '@angular/core';
import { ProposedCategory } from 'src/app/models/proposed-recipe/proposed-category.model';
import { AlertService } from 'src/app/services/app-services/alert.service';
import { CategoryService } from 'src/app/services/category.service';
import { WindowService } from 'src/app/services/window.service';

@Component({
  selector: 'app-propose-category-modal',
  templateUrl: './propose-category-modal.component.html',
  styleUrls: ['./propose-category-modal.component.scss']
})
export class ProposeCategoryModalComponent implements OnInit {
  @Input() public proposedCategory: ProposedCategory;
  private previousName: string;

  constructor(
    private windowService: WindowService, 
    private alertService: AlertService,
    private categoryService: CategoryService) { }

  ngOnInit(): void {
    this.previousName = this.proposedCategory.name;
  }

  close(){
    this.windowService.proposeCategoryWindowOpen.next(false);
  }

  cancel(){
    if(this.previousName){
      this.proposedCategory.name = this.previousName;
    }else{
      this.proposedCategory = null;
      this.categoryService.proposeCategoryModalCancel.next(this.proposedCategory);
    }

    this.close();
  }

  submit(){
    if(!this.validate()){
      return;
    }

    this.categoryService.proposeCategoryModal.next(this.proposedCategory);
    this.close();
  }

  validate(): boolean{
    if(!this.proposedCategory.name || this.proposedCategory.name.length < 3){
      this.alertService.showError("Nazwa powinna zawierać conajmniej 3 znaki");
      return false;
    }

    return true;
  }
}
