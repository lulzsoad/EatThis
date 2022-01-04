export class Bookmark{
    public name: string;
    public isActive: boolean;
    public label: string;
    public isDisabled: boolean

    constructor(name: string, label: string, isActive: boolean = false, isDisabled: boolean = false){
        this.name = name;
        this.isActive = !isDisabled ? isActive : false;
        this.label = label;
        this.isDisabled = isDisabled;
        
    }
}