import { Directive, ElementRef, HostBinding, HostListener } from "@angular/core";

@Directive({
    selector: '[requiredField]'
})
export class RequiredFieldDirective{
    @HostBinding('element') elementRef: ElementRef;
    @HostListener('focusout') validate(){
        console.log(this.elementRef);
        let isOk = this.isInputOk(this.elementRef);

        if(!isOk){
            this.elementRef.nativeElement.classList.remove('border-green');
            this.elementRef.nativeElement.classList.add("border-danger");
        }else{
            this.elementRef.nativeElement.classList.remove('border-danger');
            this.elementRef.nativeElement.classList.add("border-green");
        } 
    }

    private isInputOk(elementRef: ElementRef): boolean{
        let element = elementRef.nativeElement;
        let value = element.value;

        if(value.length < 1){
            return false;
        }
        if(element.type == 'email'){
            if(!value.includes('@') || !value.incldes('.') || value.length < 5){
                return false;
            }
        }
        return true;
    }

    constructor(elementRef: ElementRef) {
        this.elementRef = elementRef
    }
}