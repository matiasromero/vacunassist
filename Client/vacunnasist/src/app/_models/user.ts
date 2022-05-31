import { AppliedVaccine } from "./applied-vaccine";

export class User {
    id!: string;
    username!: string;
    address!: string;
    password!: string;
    fullName!: string;
    gender!: string;
    phoneNumber!: string;
    email!: string;
    birthdate!: Date;
    dni!: string;
    belongsToRiskGroup: boolean = false;
    role!: string;
    token!: string;
    vaccines: AppliedVaccine[] = [];
    
    public isPatient(): boolean {
         return this.role === 'patient';
    }
}