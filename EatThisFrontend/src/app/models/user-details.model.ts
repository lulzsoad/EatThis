export class UserDetails{
    id: number;
    firstName: string;
    lastName: string;
    email: string;
    dateOfBirth: Date;
    registerDate: Date;
    roleId: number;
    image: string;
    description: string;
    fullName: string;

    constructor(
        firstName: string = null,
        lastName: string = null,
        email: string = null,
        dateOfBirth: Date = null,
        registerDate: Date = null,
        roleId: number = null,
        image: string = null,
        description: string = null
    ){
        this.firstName = firstName,
        this.lastName = lastName,
        this.email = email,
        this.dateOfBirth = dateOfBirth,
        this.registerDate = registerDate,
        this.roleId = roleId,
        this.image = image,
        this.description = description,
        this.fullName = `${this.firstName} ${this.lastName}`
    }
}