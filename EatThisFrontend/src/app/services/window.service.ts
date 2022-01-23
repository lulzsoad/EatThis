import { Injectable } from "@angular/core";
import { Subject } from "rxjs";

@Injectable()
export class WindowService{
    public openWindow: Subject<boolean> = new Subject<boolean>();
    public closeWindow: Subject<boolean> = new Subject<boolean>();
    public proposeIngredientWindowOpen: Subject<boolean> = new Subject<boolean>();
    public proposeCategoryWindowOpen: Subject<boolean> = new Subject<boolean>();
    public deleteObject: Subject<DeleteObject> = new Subject<DeleteObject>();
}

export class DeleteObject{
    id: number;
    name: string;
}