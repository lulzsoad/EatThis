import { Component, Input, OnInit } from '@angular/core';
import { DeleteObject, WindowService } from 'src/app/services/window.service';

@Component({
  selector: 'app-delete-object-modal',
  templateUrl: './delete-object-modal.component.html',
  styleUrls: ['./delete-object-modal.component.scss']
})
export class DeleteObjectModalComponent implements OnInit {
  @Input() opened: boolean;
  @Input() object: DeleteObject;

  constructor(private windowService: WindowService) { }

  ngOnInit(): void {
  }

  public close() {
    this.windowService.closeWindow.next(false);
    this.opened = false;
  }

  public handleResult(value: boolean){
    if(value){
      this.windowService.deleteObject.next(this.object);
      this.close();
    }else{
      this.close();
    }
  }

  public open() {
    this.opened = true;
  }
}
