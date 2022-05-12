export class User {
    id!: string;
    username!: string;
    address!: string;
    password!: string;
    belongsToRiskGroup: boolean = false;
    role!: string;
    token!: string;
}