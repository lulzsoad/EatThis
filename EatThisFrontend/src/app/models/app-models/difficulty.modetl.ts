export class Difficulty{
    public text: string;
    public value: number;

    constructor(text: string, value: number){
        this.text = text;
        this.value = value;
    }
}

export class RecipeDifficulties{
    public static difficuilties = [
        "Łatwy",
        "Normalny",
        "Trudny"
    ]
}