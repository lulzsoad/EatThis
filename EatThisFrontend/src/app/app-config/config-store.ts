import { FileRestrictions } from "@progress/kendo-angular-upload";
import { Subject } from "rxjs";

export class ConfigStore{
    loadingPanel: Subject<boolean> = new Subject<boolean>();
    imageUploadRestrictions: FileRestrictions = {
        allowedExtensions: [".jpg", ".png", ".jpeg", ".bmp"],
        maxFileSize: 2097152 //2mb
      };

    startLoadingPanel(){
        this.loadingPanel.next(true);
    }

    stopLoadingPanel(){
        this.loadingPanel.next(false);
    }

    getImageUploadFileRestriction(){
        return this.imageUploadRestrictions;
    }
}