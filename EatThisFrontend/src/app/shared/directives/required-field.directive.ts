import { Directive, ElementRef, HostBinding, HostListener, OnInit } from "@angular/core";

@Directive({
    selector: '[requiredField]'
})
export class RequiredFieldDirective implements OnInit{
    @HostBinding('element') elementRef: ElementRef;
    @HostListener('focusout') validate(){
        console.log(this.elementRef);
        let isOk = this.isInputOk(this.elementRef);

        if(!isOk){
            this.elementRef.nativeElement.classList.remove('border-success');
            this.elementRef.nativeElement.classList.add("border-danger");
            this.elementRef.nativeElement.closest('div').querySelector('.validation-warning').hidden = false;
        }else{
            this.elementRef.nativeElement.classList.remove('border-danger');
            this.elementRef.nativeElement.classList.add("border-success");
            this.elementRef.nativeElement.closest('div').querySelector('.validation-warning').hidden = true;
        } 
    }

    private isInputOk(elementRef: ElementRef): boolean{
        let element = elementRef.nativeElement;
        let value = element.value;

        if(value.length < 1){
            return false;
        }
        if(element.type == 'email'){
            if(!value.includes('@') || !value.includes('.') || value.length < 5){
                return false;
            }
        }
        return true;
    }

    constructor(elementRef: ElementRef) {
        this.elementRef = elementRef;
    }
    ngOnInit(): void {
        this.elementRef.nativeElement.closest('div').querySelector('.validation-warning').hidden = true;
    }
}