export class Operation{
    // operation
    public op: string;
    // column
    public path: string;
    // value
    public value: string | Date;

    constructor(op: string, path: string, value: string | Date){
        this.op = op;
        this.path = path;
        this.value = value;
    }
}

export class OperationEnum{
    public static REPLACE = "replace";
}