import { Directive, ElementRef, HostBinding, HostListener, Input, ViewChild } from "@angular/core";

@Directive({
    selector: '[appDropdown]'
})
export class DropdownDirective{
    @HostBinding('element') elementRef: ElementRef;
    @HostListener('mouseover', ['$event']) toggleOpen(event: Event){
        this.elementRef.nativeElement.querySelector('.dropdown-menu').classList.add("show");
    }
    @HostListener('mouseleave', ['$event']) toggleClose(event: Event){
        this.elementRef.nativeElement.querySelector('.dropdown-menu').classList.remove('show');
    }

    constructor(elementRef: ElementRef) {
        this.elementRef = elementRef
    }
}